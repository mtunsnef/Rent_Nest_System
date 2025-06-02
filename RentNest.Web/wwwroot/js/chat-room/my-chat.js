var currentUserId = parseInt(document.getElementById('currentUserId').value);
var openedId = parseInt(document.getElementById('openedConversationId').value);

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

        loadConversation(chatState.currentConversationId);
    });

    if (openedId) {
        chatState.currentConversationId = openedId;
        loadConversation(openedId); 
    } else {
        $('.conversation-item').first().trigger('click');
    }
});
function loadConversation(conversationId) {
    $.ajax({
        url: `/api/v1/chatroom/detail/${conversationId}`,
        method: 'GET',
        success: function (data) {
            chatState.currentReceiverId = data.receiverId;
            bindSendButton(currentUserId);

            const activityStatus = getActivityStatus(data.isOnline, data.lastActiveAt);
            const statusDotClass = data.isOnline ? "status-online" : "status-offline";
            $("#receiverInfo").html(`
                <div class="d-flex align-items-center">
                    <img src="${data.receiverAvatarUrl}" class="rounded-circle mr-3" width="40" height="40" />
                    <div>
                        <div class="font-weight-bold">${data.receiverFullName}</div>
                        <div class="d-flex align-items-center my-2">
                            <span class="status-dot ${statusDotClass}"></span>
                            <small class="ml-2" style="line-height: 0 !important;">${activityStatus}</small>
                        </div>
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

            renderQuickReplies(data.quickReplies);
        },
        error: function () {
            alert("Không thể tải nội dung cuộc trò chuyện.");
        }
    });
}
function getActivityStatus(isOnline, lastActiveAt) {
    const online = isOnline === true || isOnline === "true";

    if (online) return "Đang hoạt động";

    if (!lastActiveAt) return "";

    const lastActive = new Date(lastActiveAt);
    if (isNaN(lastActive.getTime())) return "";

    const now = new Date();
    const diffMs = now - lastActive;

    const seconds = Math.floor(diffMs / 1000);
    const minutes = Math.floor(seconds / 60);
    const hours = Math.floor(minutes / 60);
    const days = Math.floor(hours / 24);

    if (seconds < 60) return `Hoạt động vài giây trước`;
    if (minutes < 60) return `Hoạt động ${minutes} phút trước`;
    if (hours < 24) return `Hoạt động ${hours} giờ trước`;
    if (days < 7) return `Hoạt động ${days} ngày trước`;

    return `Hoạt động vào ${lastActive.toLocaleDateString("vi-VN")}`;
}


function renderDateLabelIfNeeded(msgDate) {
    const now = new Date();
    const today = new Date(now.getFullYear(), now.getMonth(), now.getDate());
    const yesterday = new Date(today);
    yesterday.setDate(today.getDate() - 1);

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

        let messageContent = "";
        if (msg.imageUrl) {
            messageContent += `<img src="${msg.imageUrl}" class="img-fluid rounded" style="max-width: 250px;" />`;
        }
        if (msg.content) {
            messageContent += `<div>${msg.content}</div>`;
        }

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

    connection.on("ReceiveMessage", function (senderId, message, imageUrl) {
        appendMessageToChat(senderId, message, imageUrl);

        if (chatState.currentConversationId) {
            $.get(`/api/v1/chatroom/detail/${chatState.currentConversationId}`, function (data) {
                const formattedStatus = getActivityStatus(data.isOnline, data.lastActiveAt);
                $('#receiverInfo small').text(formattedStatus);
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
function readFileAsDataURL(file) {
    return new Promise((resolve, reject) => {
        const reader = new FileReader();
        reader.onload = () => resolve(reader.result);
        reader.onerror = reject;
        reader.readAsDataURL(file);
    });
}

function bindSendButton(userId) {
    const fileInput = document.getElementById("imageInput");
    const previewImageInside = document.getElementById("previewImageInside");
    const removeImageBtn = document.getElementById("removeImageBtn");
    const messageInput = document.getElementById("messageInput");

    fileInput.addEventListener("change", function () {
        const file = this.files[0];
        if (file) {
            const reader = new FileReader();
            reader.onload = function (e) {
                previewImageInside.src = e.target.result;
                previewImageInside.style.display = "flex";
                removeImageBtn.style.display = "flex";
                messageInput.style.paddingTop = "75px";
                messageInput.style.paddingBottom = "20px";
            };
            reader.readAsDataURL(file);
        }
    });

    removeImageBtn.addEventListener("click", function () {
        fileInput.value = null;
        previewImageInside.style.display = "none";
        removeImageBtn.style.display = "none";
        messageInput.style.paddingTop = "0";
        messageInput.style.paddingBottom = "0";
    });

    $("#sendBtn").off("click").on("click", function () {
        sendMessageHandler(userId);
    });

    $("#messageInput").off("keypress").on("keypress", function (e) {
        if (e.which === 13 && !e.shiftKey) {
            e.preventDefault();
            sendMessageHandler(userId);
        }
    });
}

async function sendMessageHandler(userId) {
    const fileInput = document.getElementById("imageInput");
    const previewImageInside = document.getElementById("previewImageInside");
    const removeImageBtn = document.getElementById("removeImageBtn");
    const messageInput = document.getElementById("messageInput");

    const message = messageInput.value.trim();
    const file = fileInput.files[0];

    if (!message && !file) return;
    if (!chatState.currentConversationId || !chatState.currentReceiverId) return;

    let imageUrl = null;

    try {
        if (file) {
            imageUrl = await readFileAsDataURL(file);
        }

        await connection.invoke("SendMessage", chatState.currentConversationId, chatState.currentReceiverId, message, imageUrl);

        appendMessageToChat(userId, message, imageUrl);

        messageInput.value = "";
        fileInput.value = null;
        previewImageInside.style.display = "none";
        removeImageBtn.style.display = "none";
        messageInput.style.paddingTop = "12px";

    } catch (err) {
        console.error("SendMessage error:", err);
    }
}


function appendMessageToChat(senderId, message, imageUrl) {
    const isMine = senderId === currentUserId;
    const now = new Date();
    const timeOnly = now.toLocaleTimeString('vi-VN', { hour: '2-digit', minute: '2-digit' });

    const dateHtml = renderDateLabelIfNeeded(now);
    let messageContent = "";
    if (imageUrl) {
        messageContent += `<img src="${imageUrl}" class="img-fluid rounded" style="max-width: 250px;" />`;
    }
    if (message) {
        messageContent += `<div>${message}</div>`;
    }

    const html = dateHtml + renderMessageBubble(isMine, messageContent, timeOnly);

    $('#chatBox').append(html);
    $('#chatBox').scrollTop($('#chatBox')[0].scrollHeight);
}
function renderQuickReplies(quickReplies) {
    const quickRepliesHtml = quickReplies.map(q => {
        return `<button class="btn btn-outline-secondary btn-sm mr-2 mb-2 quick-reply-btn">${q.message}</button>`;
    }).join('');
    $('#quickReplyContainer').html(quickRepliesHtml);

    $('.quick-reply-btn').off('click').on('click', async function () {
        const message = $(this).text();
        $('#messageInput').val(message);
        sendMessageHandler(currentUserId);
    });
}
