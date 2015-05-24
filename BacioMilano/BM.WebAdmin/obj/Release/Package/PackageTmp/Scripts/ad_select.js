$.fn.extend({

    getSize: function () {
        return $(this).get(0).options.length;
    },

    getSelectedText: function () {
        if (this.getSize() > 0) {
            var index = $(this).val();
            return $(this).get(0).options[index].text;
        }
    },

    getSelectedValue: function () {
        if (this.getSize() > 0) {
            return $(this).val();
        }
    },

    setSelectedValue: function (value) {
        $(this).get(0).value = value;
    },

    setSelectedText: function (text) {
        var isExist = false;
        var count = this.getSize();
        for (var i = 0; i < count; i++) {
            if ($(this).get(0).options[i].text == text) {
                $(this).get(0).options[i].selected = true;
                isExist = true;
                break;
            }
        }
        return isExist;
    },

    setSelectedIndex: function (index) {
        var count = this.getSize();
        if (index >= count || index < 0) {
            return;
        }
        else {
            $(this).get(0).selectedIndex = index;
        }
    },

    isExistItem: function (value) {
        var isExist = false;
        if ($(this).size() == 0)
            return isExist;
        var count = this.getSize();
        for (var i = 0; i < count; i++) {
            if ($(this).get(0).options[i].value == value) {
                isExist = true;
                break;
            }
        }
        return isExist;
    },

    addOption: function (text, value) {
        if (!this.isExistItem(value)) {
            var id = $(this).attr("id");
            if ($.ad.isDefined(id)) {
                var obj = document.getElementById(id);
                if ($.ad.isDefined(obj.options)) {
                    obj.options.add(new Option(text, value));
                }
            }
        }
    },

    removeItem: function (value) {
        if (this.isExistItem(value)) {
            var count = this.getSize();
            for (var i = 0; i < count; i++) {
                if ($(this).get(0).options[i].value == value) {
                    $(this).get(0).remove(i);
                    break;
                }
            }
        }
        else {
            alert("待删除的项不存在!");
        }
    },

    removeIndex: function (index) {
        var count = this.getSize();
        if (index >= count || index < 0) {
            return;
        }
        else {
            this.get(0).remove(index);
        }
    },

    removeSelected: function () {
        var index = this.getSelectedIndex();
        this.removeIndex(index);
    },

    clearAll: function () {
        this.get(0).options.length = 0;
    }
});