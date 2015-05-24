String.prototype.trimStart = function () {
    var str = this;
    var nLen = str.length;
    var i = 0;
    for (var n = 1; n <= nLen; n++)
        if (str.substr(n - 1, 1) == " ")
            i++;
        else
            break;
    return str.substr(i, nLen);
}
String.prototype.trimEnd = function () {
    var str = this;
    var nLen = str.length;
    for (var i = str.length - 1; i >= 0; i--)
        if (str.substr(i, 1) != " ")
            break;
        else
            nLen--;
    return str.substr(0, nLen);
}
String.prototype.trim = function () {
    return this.trimStart().trimEnd();
}
String.prototype.replaceAll = function (oldstr, replacestr) {
    var str = this;
    while (str.indexOf(oldstr) != -1)
        str = str.replace(oldstr, replacestr);
    return str;
}
String.prototype.lenBytes = function () {
    var str = this;
    var byteLen = 0, len = str.length;
    if (str) {
        for (var i = 0; i < len; i++) {
            if (str.charCodeAt(i) > 255) {
                byteLen += 2;
            }
            else {
                byteLen++;
            }
        }
        return byteLen;
    }
    else {
        return 0;
    }
}