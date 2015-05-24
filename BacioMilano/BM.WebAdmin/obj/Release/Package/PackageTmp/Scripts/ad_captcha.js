jQuery.adCaptcha = {
    refresh: function (id, style) {
        $.post("/AdCtrl/Captcha_v", { id: id, style: style }).done(function (data) {
            $("#" + id + "_hid").val(data);
            var src = $("#" + id + "_img").attr("src");
            var arr = src.split("=")
            src = arr[0] + "=" + data;
            $("#" + id + "_img").attr("src", src);
        });
    }
};