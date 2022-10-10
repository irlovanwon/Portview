Irlovan.Operator = {
    IIF: function (value) {
        if ((value == "true") || (value == true) || (value == "True") || (value == "TRUE")) {
            return true;
        } else {
            return false;
        }
    }
}