var currentUserId = parseInt(document.getElementById('currentUserId').value);

const chatState = {
    currentConversationId: null,
    currentReceiverId: null,
};

let connection;
let lastMessageDate = null;

$(document).ready(function () {
    initChatHub(currentUserId);

    $('.conversation-item').on('click', function () {
        chatState.currentConversationId = $(this).data("id");
        chatState.currentReceiverId = $(this).data("receiver-id");

        $.ajax({
            url: `/api/v1/chatroom/detail/${chatState.currentConversationId}`,
            method: 'GET',
            success: function (data) {
                console.log("DATA from /chatroom/detail API:", data);

                chatState.currentReceiverId = data.receiverId;
                bindSendButton(currentUserId);

                $("#receiverInfo").html(`
                    <div class="d-flex align-items-center">
                        <img src="${data.receiverAvatarUrl}" class="rounded-circle mr-3" width="40" height="40" />
                        <div>
                            <div class="font-weight-bold">${data.receiverFullName}</div>
                            <small>${data.lastActiveAt}</small>
                        </div>
                    </div>
                `);

                if (data.postId != null) {
                    $('#postInfo').removeClass('d-none').html(`
                        <div class="d-flex align-items-center">
                            <img src="${data.postImageUrl}" class="mr-3" width="60" height="60" style="object-fit: cover;" />
                            <div>
                                <div class="font-weight-bold">${data.postTitle}</div>
                                <div class="text-muted">${data.postPrice ? data.postPrice.toLocaleString() + ' VNĐ/tháng' : ''}</div>
                            </div>
                        </div>
                    `);
                } else {
                    $('#postInfo').removeClass('d-flex');
                    $('#postInfo').addClass('d-none');
                }

                const messagesHtml = renderMessages(data.messages, currentUserId);
                $('#chatBox').html(messagesHtml);
                $('#chatBox').scrollTop($('#chatBox')[0].scrollHeight);

            },
            error: function () {
                alert("Không thể tải nội dung cuộc trò chuyện.");
            }
        });
    });

    $('.conversation-item').first().trigger('click');
});

function renderDateLabelIfNeeded(msgDate) {
    const now = new Date();
    const today = new Date(now.getFullYear(), now.getMonth(), now.getDate()); // 0h hôm nay
    const yesterday = new Date(today);
    yesterday.setDate(today.getDate() - 1); // 0h hôm qua

    const messageDate = new Date(msgDate.getFullYear(), msgDate.getMonth(), msgDate.getDate());

    if (!lastMessageDate || messageDate.getTime() !== lastMessageDate.getTime()) {
        let dateLabel = '';
        if (messageDate.getTime() === today.getTime()) {
            dateLabel = 'Hôm nay';
        } else if (messageDate.getTime() === yesterday.getTime()) {
            dateLabel = 'Hôm qua';
        } else {
            dateLabel = msgDate.toLocaleDateString('vi-VN');
        }

        lastMessageDate = messageDate;
        return `
            <div class="text-center text-muted my-2">
                <small>${dateLabel}</small>
            </div>`;
    }

    return '';
}


function renderMessageBubble(isMine, messageContent, timeOnly) {
    return `
        <div class="d-flex ${isMine ? 'justify-content-end' : 'justify-content-start'} mb-2">
            <div class="p-2 ${isMine ? 'bg-primary text-white' : 'bg-light'} rounded">
                ${messageContent}
                <div><small>${timeOnly}</small></div>
            </div>
        </div>`;
}

function renderMessages(messages, currentUserId) {
    let messagesHtml = '';
    lastMessageDate = null;

    messages.forEach(msg => {
        const msgDate = new Date(msg.sentAt);
        const timeOnly = msgDate.toLocaleTimeString('vi-VN', { hour: '2-digit', minute: '2-digit' });
        const isMine = msg.senderId === currentUserId;

        const messageContent = msg.content.startsWith("[image]")
            ? `<img src="${msg.content.replace("[image]", "")}" class="img-fluid rounded" style="max-width: 250px;" />`
            : msg.content;

        messagesHtml += renderDateLabelIfNeeded(msgDate);
        messagesHtml += renderMessageBubble(isMine, messageContent, timeOnly);
    });

    return messagesHtml;
}

async function initChatHub(userId) {
    connection = new signalR.HubConnectionBuilder()
        .withUrl("/chathub")
        .withAutomaticReconnect()
        .build();

    connection.on("ReceiveMessage", (senderId, message) => {
        appendMessageToChat(senderId, message);

        if (chatState.currentConversationId) {
            $.get(`/api/v1/chatroom/detail/${chatState.currentConversationId}`, function (data) {
                $('#receiverInfo small').text(data.lastActiveAt);
            });
        }
    });

    try {
        await connection.start();
        console.log("SignalR connected");
    } catch (err) {
        console.error("SignalR connection error:", err);
    }
}
function bindSendButton(userId) {
    async function sendMessageHandler() {
        const message = $("#messageInput").val().trim();
        if (!message || !chatState.currentConversationId || !chatState.currentReceiverId) return;

        try {
            await connection.invoke("SendMessage", chatState.currentConversationId, chatState.currentReceiverId, message);
            appendMessageToChat(userId, message);
            $("#messageInput").val("");
        } catch (err) {
            console.error("SendMessage error:", err);
        }
    }

    $("#sendBtn").off("click").on("click", sendMessageHandler);
    $("#messageInput").off("keypress").on("keypress", function (e) {
        if (e.which === 13 && !e.shiftKey) {
            e.preventDefault();
            sendMessageHandler();
        }
    });
}
function appendMessageToChat(senderId, message) {
    const isMine = senderId === currentUserId;
    const now = new Date();
    const timeOnly = now.toLocaleTimeString('vi-VN', { hour: '2-digit', minute: '2-digit' });

    const dateHtml = renderDateLabelIfNeeded(now);
    const messageContent = message.startsWith("[image]")
        ? `<img src="${message.replace("[image]", "")}" class="img-fluid rounded" style="max-width: 250px;" />`
        : message;

    const html = dateHtml + renderMessageBubble(isMine, messageContent, timeOnly);

    $('#chatBox').append(html);
    $('#chatBox').scrollTop($('#chatBox')[0].scrollHeight);
}
