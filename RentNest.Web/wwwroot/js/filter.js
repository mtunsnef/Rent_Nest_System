$(document).ready(function () {
    $('#filterButton').click(function () {
        const filters = {
            roomType: $('#roomType').val(),
            roomStatus: $('#roomStatus').val(),
            minPrice: $('#minPrice').val(),
            maxPrice: $('#maxPrice').val(),
            minArea: $('#minArea').val(),
            maxArea: $('#maxArea').val()
        };

        $.ajax({
            url: '/Room/FilterRooms',
            type: 'GET',
            data: filters,
            success: function (html) {
                // Lấy ra phần room-list trong HTML trả về rồi gắn vào
                const newRoomList = $(html).find('#room-list').html();
                $('#room-list').html(newRoomList);
            },
            error: function () {
                alert("Lỗi khi lọc phòng.");
            }
        });
    });
});