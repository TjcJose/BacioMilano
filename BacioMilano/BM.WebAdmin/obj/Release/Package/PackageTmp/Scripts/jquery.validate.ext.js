$.validator.prototype.showLabel_style1 = $.validator.prototype.showLabel;

$.validator.prototype.showLabel_style2 = function (element, msg) {
    if ($.ad.isDefined(msg)) {
        msg = "<span class='glyphicon glyphicon-exclamation-sign' title='" + msg + "' />";
    }
    var label = this.errorsFor(element);
    if (label.length) {
        // refresh error/success class
        label.removeClass(this.settings.validClass).addClass(this.settings.errorClass);
        // replace message on existing label
        label.html(msg);
    } else {
        // create label
        label = $("<" + this.settings.errorElement + ">")
            .attr("for", this.idOrName(element))
            .addClass(this.settings.errorClass)
            .html(msg || "");
        if (this.settings.wrapper) {
            // make sure the element is visible, even in IE
            // actually showing the wrapped element is handled elsewhere
            label = label.hide().show().wrap("<" + this.settings.wrapper + "/>").parent();
        }
        if (!this.labelContainer.append(label).length) {
            if (this.settings.errorPlacement) {
                this.settings.errorPlacement(label, $(element));
            } else {
                label.insertAfter(element);
            }
        }
    }
    if (!msg && this.settings.success) {
        label.text("");
        if (typeof this.settings.success === "string") {
            label.addClass(this.settings.success);
        } else {
            this.settings.success(label, element);
        }
    }
    this.toShow = this.toShow.add(label);
};

