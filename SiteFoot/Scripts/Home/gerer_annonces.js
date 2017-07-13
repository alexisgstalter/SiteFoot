$(document).ready(function () {
   /* var fd = new FormData();
    var compteur = 0;
    var num_accord = $('.table tr td').eq(1).text();
    console.log(num_accord);
    $(".piece_jointe").each(function () {

        var piece = $(this);
        var ins = document.getElementById('pj').files.length;
        for (var x = 0; x < ins; x++) {
            fd.append("fileToUpload" + compteur, document.getElementById('pj').files[x]);
            compteur++;
        }
    });
    fd.append("num_accord", num_accord);

    var ajaxRequest = $.ajax({
        type: "POST",
        url: "/Upload/SavePieceJointeExpertise",
        async: false,
        contentType: false,
        processData: false,
        data: fd
    });*/
    Load();
    function Load() {
        $("#annonces_container").empty();
        $.ajax({
            url: "/Annonce/GetGestionAnnonces",
            type: "POST",
            data: JSON.stringify({  }),
            contentType: "application/json; charset=utf-8",
            dataType: "json",

            success: function (data) {
                if (data.ok) {
                    $("#annonces_container").append(data.html);
                    $("table").dataTable();
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
    }
})