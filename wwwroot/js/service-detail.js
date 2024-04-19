$(document).ready(function () {
    $('#time-select').change(function () {
        var selectedTime = $(this).val();
        $('#selected-time').val(selectedTime);
    });

    var data = JSON.parse($('#DataList').val())
    $("#time-select").select2({
        placeholder: 'Chọn thời gian',
        data: data,
        closeOnSelect: true,
        allowClear: true
    });

    $('#btn-book-time').on('click', function (e) {
        e.preventDefault();
        var errorMessage = $('#ErrorMessage');
        errorMessage.hide();
        
        var data = {
            IdServie: $('#IdService').val(),
            Email: $('#Email').val(),
            Time: $('#time-select').val(),
            Price: $('#Price').val(),
            DateTime: $('#date').val(),
        }
        
        var check = true;
        var html = '';
        
        if (data.Time === '' || data.Time === null || data.Time === undefined){
            html += 'Vui lòng chọn khung giờ <br>';
            check = false;
        }
        if (data.DateTime === ''){
            html += 'Vui lòng chọn ngày hẹn <br>';
            check = false;
        }
        
        var currentTime = new Date();
        var formattedDateSelected = formatDate(data.DateTime);
        var currentDateFormatted = ("0" + currentTime.getDate()).slice(-2) + "/" + ("0" + (currentTime.getMonth() + 1)).slice(-2) + "/" + currentTime.getFullYear().toString();

        if (new Date(currentDateFormatted).getTime() > new Date(data.DateTime).getTime()) {
            html += 'Ngày bạn chọn không hợp lệ. <br>';
            check = false;
        } else if (currentDateFormatted === formattedDateSelected) {
            var selectedTime = data.Time.replaceAll('h', ':');

            if (isValidTime(selectedTime)) {
                console.log('Khung giờ hợp lệ.');
            } else {
                html += 'Khung giờ bạn chọn không hợp lệ. <br>';
                check = false;
            }
        }
        
        errorMessage.html(html);
        errorMessage.show();
        
        if (check){
            $.ajax({
                cache: false,
                url: '/Service/DatLich',
                type: 'POST',
                data: data,
                success: function (res) {
                    window.location.href = '/Service/LichDaDat'
                },
                error: function (err) {
                    errorMessage.html('Có lỗi xảy ra. Vui lòng thử lại');
                }
            });
        }
    })
});

var formatDate = function (dateString) {
    var date = new Date(dateString);

    var day = ("0" + date.getDate()).slice(-2);
    var month = ("0" + (date.getMonth() + 1)).slice(-2);
    var year = date.getFullYear();

    return day + "/" + month + "/" + year;
}

var isValidTime = function (selectedTime) {
    // Lấy giờ hiện tại
    var currentTime = new Date();
    var currentHours = currentTime.getHours();
    var currentMinutes = currentTime.getMinutes();

    // Tách giờ và phút từ khung giờ chọn
    var selectedHours = parseInt(selectedTime.split(':')[0]);
    var selectedMinutes = parseInt(selectedTime.split(':')[1]);

    // Tính toán thời gian hiện tại thành phút
    var currentTotalMinutes = currentHours * 60 + currentMinutes;
    // Tính toán thời gian chọn thành phút
    var selectedTotalMinutes = selectedHours * 60 + selectedMinutes;

    // Kiểm tra nếu thời gian chọn trong quá khứ
    if (selectedTotalMinutes < currentTotalMinutes) {
        return false;
    }

    return true;
}