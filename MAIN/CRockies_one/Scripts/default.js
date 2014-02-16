var Default = {
    playerSelect: null,
    init: function (id,horp) {
        var _this = this;

        _this.playerSelect = $('#pr');
        //this.playerSelect.html('<option value="0">Loading...</option>');
        
        _this.playerSelect.change(function () {
            _this.prChange(_this.playerSelect.val());
        });
        $.post('/Home/Players/', { id: id, horp: horp }, function (result) {
            _this.playerSelect.html(HtmlRenderer.prOptions(result.p));
            //_this.playerSelect.trigger('change');
        }, 'json');
    },


    getParameterByName: function(name) {
            name = name.replace(/[\[]/, "\\\[").replace(/[\]]/, "\\\]");
            var regex = new RegExp("[\\?&]" + name + "=([^&#]*)"),
                results = regex.exec(location.search);
            return results == null ? "" : decodeURIComponent(results[1].replace(/\+/g, " "));
    },

    prChange: function (val) {
        var _this = this;

        if (val > 0) {
            //CookieJar.setCookie('player', val);

        }
    }
};

var HtmlRenderer = {
    prOptions: function (data) {
        var player = CookieJar.getCookie('player');
        if (player)
            player = parseInt(pr);

        var html = '<option value="0"></option>';
        var html;
        $.each(data, function (i, o) {
            if ((player != null) && (player == o.Player_ID)) {
                //html += '<option value="' + o.PlayerId + '" selected="selected">';
                //html += '<option value="' + o.Player_ID + '">';
                html += '<option value="' + o.PlayerId + '">';
                html += o.Firstname + ' ' + o.Lastname;
                //html += '@Html.ActionLink(' + o.Firstname + ' ' + o.Lastname + ',"PitcherProfile", new { id = ' + o.PlayerId + '}) ';
                html += '</option>';
            } else {
                html += '<option value="' + o.PlayerId + '">';
                html += o.Firstname + ' ' + o.Lastname;
                html += '</option>';
            }
        });

        return html;
    }
};
var CookieJar = {
    setCookie: function (name, value) {
        document.cookie = name + '=' + value;
    },

    getCookie: function (name) {
        var cookies = document.cookie.split(';');

        if (cookies.length > 0) {
            var pattern = new RegExp('^' + name + '=');

            for (var i in cookies) {
                // Trim spaces
                var cookie = cookies[i].replace(/\s/, '');

                if (cookie.match(pattern)) {
                    var arr = cookie.split('=');

                    if (arr.length > 1)
                        return arr[1];
                }
            }
        }

        return null;
    },

    delCookie: function (name) {
        var date = new Date();

        date.setDate(date.getDate() - 7);
        document.cookie = name + '=; expires=' + date.toGMTString();
    }
};