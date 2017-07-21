$(document).ready(function () {
    var offset = 0;
    var win = $(window);
    $.ajax({
        url: '/Annonce/GetAnnonces',
        dataType: 'json',
        type: "POST",
        contentType : "application/json;charset=utf-8",
        data: JSON.stringify({ offset: offset }),
        success: function (data) {
            $('#posts').append(data.html);
            offset += 5;
        }
    });
    // Each time the user scrolls
    win.scroll(function () {
        var terme_recherhe = $("#terme").val();
        // End of the document reached?
        if ($(document).height() - win.height() == win.scrollTop()) {

            $.ajax({
                url: '/Annonce/GetAnnonces',
                dataType: 'json',
                type: "POST",
                contentType: "application/json;charset=utf-8",
                data: JSON.stringify({ offset : offset, term : terme_recherhe }),
                success: function (data) {
                    if (data.ok) {
                        $('#posts').append(data.html);
                        offset += 5;
                    }
                }
            });

            $.ajax({
                url: '/Annonce/GetAnnoncesByTerme',
                dataType: 'json',
                type: "POST",
                contentType: "application/json;charset=utf-8",
                data: JSON.stringify({ offset: offset }),
                success: function (data) {
                    if (data.ok) {
                        $('#posts').append(data.html);
                        offset += 5;
                    }
                }
            });

        }
    });


    $("#recherche_terme").submit(function () {
        var offset = 0;
        
        var terme_recherhe = $("#terme").val();
        $('#posts').empty();
        $.ajax({
            url: '/Annonce/GetAnnoncesByTerme',
            dataType: 'json',
            type: "POST",
            contentType: "application/json;charset=utf-8",
            data: JSON.stringify({ offset: offset, term : terme_recherhe }),
            success: function (data) {
                $('#posts').append(data.html);
                offset += 5;
            }
        });
        return false;
    });


});