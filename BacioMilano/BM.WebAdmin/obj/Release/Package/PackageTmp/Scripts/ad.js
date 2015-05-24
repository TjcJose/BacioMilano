function dcall(arg1,arg2,arg3,arg4) {
}

jQuery.ad = {
    _dialog_modal_index: 0,
    increase_dialog_modal_index: function () {
        this._dialog_modal_index++;
        return this._dialog_modal_index;
    },
    decrease_dialog_modal_index: function () {
        this._dialog_modal_index--;
        return this._dialog_modal_index;
    },
    isDefined: function (obj) {
        var objType = typeof (obj);
        return !(objType == "undefined" || objType == "unknown" || obj == null);
    },
    isNullStr: function (str) {
        return str == null || str == "";
    },
    isChecked: function (checkBoxId) {
        return document.getElementById(checkBoxId).checked
    },
    dialog_close: function () {
        $('#dialog_modal_id_' + this._dialog_modal_index).modal('hide');
    },
    dialog_modal: function (url, title, content, fnHidden, width) {
        var dialog_modal_index = $.ad.increase_dialog_modal_index();
        var containCtrlId = "dialog_modal_contain_id_" + dialog_modal_index;
        if ($("#" + containCtrlId).size() == 0) {
            $("body").append("<div id='" + containCtrlId + "' />");
        }
        var tempFn = doT.template($("#template_dialog_modal_id").html());
        var html = tempFn({ title: title, content: content, index: dialog_modal_index });
        $("#" + containCtrlId).html(html);

        $('#dialog_modal_id_' + dialog_modal_index).on('shown.bs.modal', function (e) {
            $("#dialog_modal_content_" + dialog_modal_index).load(url);
        });
        $('#dialog_modal_id_' + dialog_modal_index).on('hidden.bs.modal', function (e) {
            if ($.ad.isDefined(fnHidden)) {
                fnHidden(e);
            }
            $("#" + containCtrlId).remove();
            $.ad.decrease_dialog_modal_index();
        });
        $.ad.setOperValueFalse();
        var model = $('#dialog_modal_id_' + dialog_modal_index);
        if ($.ad.isDefined(width)) {
            model.on('shown.bs.modal', function (e) {
                model.find(".modal-dialog").width(width);
            })
        }
        model.modal({});
    },
    split_page: function (pageIndex, pageCount, pageSize, recordCount, pageIndexId, form_id, splitpage_contain_id, funSubmit) {
        var options = {
            currentPage: pageIndex,
            totalPages: pageCount,
            useBootstrapTooltip: true,
            tooltipTitles: function (type, page, current) {
                switch (type) {
                    case "first":
                        return "第一页 (共" + pageCount + "页、" + recordCount + "条记录)";
                    case "prev":
                        return "前一页 (共" + pageCount + "页、" + recordCount + "条记录)";
                    case "next":
                        return "下一页 (共" + pageCount + "页、" + recordCount + "条记录)";
                    case "last":
                        return "最后一页 (共" + pageCount + "页、" + recordCount + "条记录)";
                    case "page":
                        return "第 " + page + " 页 (共" + pageCount + "页、" + recordCount + "条记录)";
                }
            },
            bootstrapTooltipOptions: {
                html: true,
                placement: 'bottom'
            },
            onPageClicked: function (e, originalEvent, type, page) {
                $('#' + pageIndexId).val(page);
                if ($.ad.isDefined(funSubmit)) {
                    funSubmit(form_id);
                }
                else {
                    $('#' + form_id).submit();
                }
            }
        }
        $('#' + splitpage_contain_id).bootstrapPaginator(options);
    },
    alertOK: function (msg) {
        $.dialog({
            icon: 'succeed',
            content: msg,
            lock: true,
            opacity: 0.5
        });
    },
    alertWarn: function (msg) {
        $.dialog({
            icon: 'warning',
            content: msg,
            lock: true,
            opacity: 0.5
        });
    },
    alertError: function (msg) {
        $.dialog({
            icon: 'error',
            content: msg,
            lock: true,
            opacity: 0.5
        });
    },
    confirm: function (title, msg, okFun, okValue, cancelFun, cancelVal) {
        if (!$.ad.isDefined(title)) {
            title = "消息";
        }
        if (!$.ad.isDefined(okValue)) {
            okValue = "确认";
        }
        if (!$.ad.isDefined(cancelFun)) {
            cancelFun = true;
        }
        if (!$.ad.isDefined(cancelVal)) {
            cancelVal = "取消";
        }

        $.dialog({
            content: msg,
            okVal: okValue,
            ok: okFun,
            cancelVal: cancelVal,
            cancel: cancelFun,
            title: title,
            lock: true,
            opacity: 0.5
        });
    },
    wait: function (msg) {
        if (!$.ad.isDefined(msg)) {
            msg = "正在操作，请稍后...";
        }
        $.blockUI({
            message: "<i class='icon-spinner icon-spin icon-2x'></i> " + msg,
            css: { "padding": "10px" }
        });
    },
    _refreshPage: false,
    setRefreshPage: function (isRefresh) {
        this._refreshPage = isRefresh;
    },
    getRefreshPage: function () {
        return this._refreshPage;
    },
    _value: {},
    getValue: function (key, defaultValue) {
        if ($.ad.isDefined(this._value[key])) {
            return this._value[key];
        }
        return defaultValue;
    },
    setValue: function (key, value) {
        this._value[key] = value;
    },
    clearValue: function () {
        delete this._value;
        this._value = {};
    },
    setOperValueFalse: function () {
        this.setValue("_oper", false);
    },
    setOperValueTrue: function () {
        this.setValue("_oper", true);
    },
    getOperValue: function () {
        return this.getValue("_oper");
    },
    setModelValue: function (model) {
        this.setValue("_model", model);
    },
    getModelValue: function () {
        return this.getValue("_model");
    },
    setStringValue: function (str) {
        this.setValue("_String", str);
    },
    getStringValue: function () {
        return this.getValue("_String");
    },
    setIntValue: function (i) {
        this.setValue("_Int", i);
    },
    getIntValue: function () {
        return this.getValue("_Int");
    },
    getFileExtName: function (filePath) {
        var reg = /(\\+)/g;
        var pfn = filePath.replace(reg, "#");
        var arrpfn = pfn.split("#");
        var fn = arrpfn[arrpfn.length - 1];
        var arrfn = fn.split(".");
        var s = arrfn[arrfn.length - 1];
        return s.toLowerCase();
    },
    isInFileExtName: function (filePath, arr) {
        var ext = $.ad.getFileExtName(filePath);
        var isHaveExt = false;
        for (var i = 0; i < arr.length; i++) {
            if (arr[i] == ext) {
                isHaveExt = true;
            }
        }
        return isHaveExt;
    },
    bindSelectInputText: function (selId, inputId) {
        var sel = $("#" + selId);
        var txt = $("#" + inputId);
        $(sel).change(function () {
            txt.val(sel.getSelectedText());
        });
    },
    getPageCount: function (recordCount, pageSize) {
        return parseInt(recordCount / pageSize) + (recordCount % pageSize > 0 ? 1 : 0);
    },
    findValueFromObjects: function (objs, name) {
        for(var i = 0; i < objs.length; i++)
        {
            var obj = objs[i];
            if(obj.name == name)
            {
                return obj.value;
            }
        }
        return "";
    },
    menu_panelShow: function (obj, downClass, upClass) {
        if (!$.ad.isDefined(downClass)) {
            downClass = "icon-double-angle-down";
        }
        if (!$.ad.isDefined(upClass)) {
            upClass = "icon-double-angle-up";
        }
        var id = $(obj).attr("id");

        var fnShow = function (ctrlId, isOld) {
            var ctrlA = $("#" + ctrlId);
            var ctrlI = ctrlA.find("i");
            var ctrlD = ctrlA.parent().next();
            if (ctrlD.hasClass("show")) {
                ctrlD.removeClass("show");
                ctrlD.addClass("hidden");
                if (ctrlI.hasClass(upClass)) {
                    ctrlI.removeClass(upClass);
                }
                if (!ctrlI.hasClass(downClass)) {
                    ctrlI.addClass(downClass);
                }

            }
            else {
                ctrlD.removeClass("hidden");
                ctrlD.addClass("show");
                if (ctrlI.hasClass(downClass)) {
                    ctrlI.removeClass(downClass);
                }
                if (!ctrlI.hasClass(upClass)) {
                    ctrlI.addClass(upClass);
                }
            }
        }
        fnShow(id, false);
    },
    menu_itemClick: function (obj, ctrl_main_id, activeClass) {
        if (!$.ad.isDefined(activeClass)) {
            activeClass = "active";
        }
        $("a[data-ad-menu='true']").each(function () {
            $(this).removeClass(activeClass);
        });
        var url = $.ad.get_ad_data_url(obj);
        $("#" + ctrl_main_id).load($.ad.get_ad_data_url(obj));
        $(obj).addClass(activeClass);
    },
    get_ad_data_url: function (obj) {
        var controller = $(obj).attr("data-ad-controller");
        var action = $(obj).attr("data-ad-action");
        var params = $(obj).attr("data-ad-params");
        var url = "";
        if (!$.ad.isNullStr(controller)) {
            url += "/" + controller;
        }
        url += "/" + action;
        if (!$.ad.isNullStr(params)) {
            url += "?" + params;
        }
        return url;
    },
    get_ad_dialog_maxWidth:function (){
        return 1000;
    },
    dialog_load_template_foot:function(templateId)
    {
        var tempFn = doT.template($("#" + templateId).html());
        var html = tempFn({});
        $("#" + templateId).parent().after(html);
    },
    set_validator_showLabel_style: function (style) {
        if (style == 1) {
            $.validator.prototype.showLabel = $.validator.prototype.showLabel_style1;
        }
        else if (style == 2) {
            $.validator.prototype.showLabel = $.validator.prototype.showLabel_style2;
        }
    }
};

