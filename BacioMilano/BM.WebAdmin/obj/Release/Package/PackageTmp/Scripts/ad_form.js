(function ($) {
    $.ad.formA = function (options) {
        var defaults =
        {
            form_id: "",
            resultCtrlId: "",
            template_id: "",//form页面内包含的javascript模板控件的id
            template_btn_id: "",//form页面内提交按钮id
            errorClass: "alert-danger",
            successClass: "alert-success",
            successTip: "操作成功",
            errorTip: "操作失败",
            url: "",
            onSuccess: null,
            onError: null,
            validatorStyle: 1
        };
        var opts = jQuery.extend(defaults, options);
        this.reset_validation = function () {
            var resultCtrl = $("#" + opts.resultCtrlId);
            resultCtrl.html("");
            if (resultCtrl.hasClass(opts.errorClass)) {
                resultCtrl.removeClass(opts.errorClass);
            }
            if (resultCtrl.hasClass(opts.successClass)) {
                resultCtrl.removeClass(opts.successClass);
            }
        },
        this.setSuccess = function (msg) {
            var resultCtrl = $("#" + opts.resultCtrlId);
            resultCtrl.text(msg);
            resultCtrl.addClass(opts.successClass);
        },
        this.setError = function (msg) {
            var resultCtrl = $("#" + opts.resultCtrlId);
            resultCtrl.text(msg);
            resultCtrl.addClass(opts.errorClass);
        },
        this.submit = function (onValid, getPostData) {
            $.ad.set_validator_showLabel_style(opts.validatorStyle);
            var form = $("#" + opts.form_id);
            if ($.ad.isNullStr(opts.url)) {
                opts.url = form.attr("action");
            }
            var resultCtrl = $("#" + opts.resultCtrlId);
            this.reset_validation();
            if (!form.valid()) {
                return;
            }
            if ($.ad.isDefined(onValid)) {
                if (!onValid()) {
                    return;
                }
            }

            $.ad.wait();
            var postData = null;
            if (!$.ad.isDefined(getPostData)) {
                postData = form.serializeArray();
            }
            else {
                postData = getPostData();
            }

            var _self = this;
            $.post(opts.url, postData).done(function (datas) {
                if (!datas.IsOK) {
                    form.validate({}).showErrors(datas.Data);
                    var msg = $.ad.isNullStr(datas.Msg) ? opts.errorTip : datas.Msg;
                    _self.setError(msg);
                    if (opts.onError != null) {
                        opts.onError(datas);
                    }
                }
                else {
                    var msg = $.ad.isNullStr(datas.Msg) ? opts.successTip : datas.Msg;
                    _self.setSuccess(msg);
                    $.ad.setOperValueTrue();
                    $.ad.setModelValue(postData);
                    if (opts.onSuccess != null) {
                        opts.onSuccess(datas);
                    }
                }
            }).complete(function (datas) {
                $.unblockUI();
            });
        },
        this.template_btn_load = function () {
            //注入操作按钮模板
            if ($("#" + opts.template_btn_id).size() == 0) {
                $.ad.dialog_load_template_foot(opts.template_id);
            }
            //unobtrusive时, ajax更新加载页面后验证失效解决方法
            $.validator.unobtrusive.parse("#" + opts.form_id);
            this.reset_validation();
        };
    };
})(jQuery);