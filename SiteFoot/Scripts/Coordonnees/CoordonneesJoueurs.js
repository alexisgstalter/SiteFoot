$("#document").ready(function () {
    $("#membre").autoComplete({
        source: function (term, response) {
            xhr = $.getJSON('/Calendrier/AutocompleteMembre', { partial_name: term }, function (data) { response(data); });
        }
    });
    $("#chercher").click(function () {
        var id_equipe = $("#equipe").val();
        var membre = $("#membre").val()
        $("#membre_container").empty();
        $.ajax({
            url: "/Coordonnees/GetMembre",
            type: "POST",
            data: JSON.stringify({ id_equipe : id_equipe, name : membre }),
            dataType: "json",
            contentType: "application/json; charset=utf-8",
            success: function (data) {
                if (data.ok) {
                    $("#membre_container").append(data.html);
                    Render();
                }
                else {
                    Materialize.toast(data.error, 3000);
                }


            },
            error: function (xhr, status, error) {
                var err = eval("(" + xhr.responseText + ")");
                alert(err.Message);
            }
        });
    });
    function Render() {
        var i = 0;
        var prevDiv = $('div.card-panel');
        $('div.card-panel').each(function () {
            if (i % 2 == 1) {
                var h1 = prevDiv.height();
                var h2 = $(this).height();
                if (h1 > h2) {
                    $(this).height(h1);
                }
                else {
                    prevDiv.height(h2);
                }
            }
            prevDiv = $(this);
            i++;
        });
    }
});