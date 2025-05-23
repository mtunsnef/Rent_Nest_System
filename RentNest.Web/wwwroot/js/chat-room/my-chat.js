function renderMessages(messages, currentUserId) {
    let messagesHtml = '';
    let lastMessageDate = null;

    messages.forEach(msg => {
        const msgDate = new Date(msg.sentAt);
        const msgDateOnly = msgDate.toISOString().split('T')[0];
        const now = new Date();
        const today = now.toISOString().split('T')[0];
        const yesterday = new Date(Date.now() - 86400000).toISOString().split('T')[0];

        if (msgDateOnly !== lastMessageDate) {
            let dateLabel = '';
            if (msgDateOnly === today) {
                dateLabel = 'Hôm nay';
            } else if (msgDateOnly === yesterday) {
                dateLabel = 'Hôm qua';
            } else {
                dateLabel = msgDate.toLocaleDateString('vi-VN');
            }

            messagesHtml += `
                        <div class="text-center text-muted my-2">
                            <small>${dateLabel}</small>
                        </div>
                    `;

            lastMessageDate = msgDateOnly;
        }

        const isMine = msg.senderId === currentUserId;
        const timeOnly = msgDate.toLocaleTimeString('vi-VN', { hour: '2-digit', minute: '2-digit' });

        messagesHtml += `
                    <div class="d-flex ${isMine ? 'justify-content-end' : 'justify-content-start'} mb-2">
                        <div class="p-2 ${isMine ? 'bg-primary text-white' : 'bg-light'} rounded">
                            ${msg.content}
                            <div><small>${timeOnly}</small></div>
                        </div>
                    </div>
                `;
    });

    return messagesHtml;
}

var currentUserId = parseInt(document.getElementById('currentUserId').value);

$(document).ready(function () {
    function updateFilterButtons() {
        $('input[name="filter"]').each(function () {
            const label = $(this).closest('label');
            if ($(this).is(':checked')) {
                label.removeClass('btn-outline-secondary').addClass('btn-primary');
            } else {
                label.removeClass('btn-primary').addClass('btn-outline-secondary');
            }
        });
    }

    updateFilterButtons();
    $('input[name="filter"]').on('change', function () {
        updateFilterButtons();
    });

    $('.conversation-item').on('click', function () {
        var conversationId = $(this).data('id');

        $.ajax({
            url: `/api/v1/chatroom/detail/${conversationId}`,
            method: 'GET',
            success: function (data) {
                $('#receiverInfo').html(`
                            <div class="d-flex align-items-center">
                                <img src="${data.receiverAvatarUrl}" class="rounded-circle mr-3" width="40" height="40" />
                                <div>
                                    <div class="font-weight-bold">${data.receiverFullName}</div>
                                    <small>${data.lastSeenText}</small>
                                </div>
                            </div>
                        `);

                if (data.postId != null) {
                    $('#postInfo').html(`
                                <div class="d-flex align-items-center">
                                    <img src="${data.postImageUrl}" class="mr-3" width="60" height="60" style="object-fit: cover;" />
                                    <div>
                                        <div class="font-weight-bold">${data.postTitle}</div>
                                        <div class="text-muted">${data.postPrice ? data.postPrice.toLocaleString() + ' VNĐ/tháng' : ''}</div>
                                    </div>
                                </div>
                            `);
                } else {
                    $('#postInfo').empty();
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

    const firstItem = $('.conversation-item').first();
    if (firstItem.length > 0) {
        firstItem.trigger('click');
    }
});