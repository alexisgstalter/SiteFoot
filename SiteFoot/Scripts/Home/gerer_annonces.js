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
    var current_id;
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
                    $("select").material_select();
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


    $("#add_modal").click(function () {
        $("#modal_ajout_annonce").modal('open');
    });


    $("#form_ajout_annonce").submit(function () {
        var titre = $("#intitule").val();
        var texte = $("#texte").val();
        $.ajax({
            url: "/Annonce/SaveGestionAnnonces",
            type: "POST",
            data: JSON.stringify({titre : titre, texte : texte}),
            contentType: "application/json; charset=utf-8",
            dataType: "json",

            success: function (data) {
                if (data.ok) {
                    var fd = new FormData();
                    var compteur = 0;
                    var id = data.id;


                    var piece = $(this);
                    var ins = document.getElementById('image').files.length;
                    for (var x = 0; x < ins; x++) {
                        fd.append("fileToUpload" + compteur, document.getElementById('image').files[x]);
                        compteur++;
                    }

                    fd.append("id_annonce", id);

                    var ajaxRequest = $.ajax({
                        type: "POST",
                        url: "/Upload/SavePieceJointeAnnonce",
                        async: false,
                        contentType: false,
                        processData: false,
                        data: fd,
                        success: function () {
                            Materialize.toast("Annonce crée", 3000);
                            $("#modal_ajout_annonce").modal('close');
                            Load();
                        }
                    });
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
        return false;
    });


    $(document).on('click', '.edit', function () {
        $("#img_container_modif").empty();
        current_id = $(this).data('id');
        $.ajax({
            url: "/Annonce/GetAnnonceById",
            type: "POST",
            data: JSON.stringify({id_annonce : current_id}),
            contentType: "application/json; charset=utf-8",
            dataType: "json",

            success: function (data) {
                if (data.ok) {
                    $("#img_container_modif").append(data.html);
                    $("#intitule_modif").val(data.titre);
                    $("#texte_modif").val(data.texte);
                    $("#id_annonce_clef").val(current_id);
                    $("#modal_modif_annonce").modal('open');
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


    $(document).on('click', '.supp', function () {
        var ligne = $(this).data("id");

        $.ajax({
            type: "POST",
            url: "/Annonce/DeleteAnnonce",
            data: '{id:"' + ligne + '"}',
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            success: function (data) {
                if (data.ok) { 
                    Materialize.toast("L'annonce a été supprimée", 3000);
                    Load();
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


    $("#form_modif_annonce").submit(function () {

        var intitule_modif = $("#intitule_modif").val();
        var texte_modif = $("#texte_modif").val();
        var id_annonce_clef = $("#id_annonce_clef").val();

        $.ajax({
            url: "/Annonce/UpdateAnnonce",
            type: "POST",
            data: JSON.stringify({ id_annonce_clef: id_annonce_clef, texte_modif: texte_modif, intitule_modif: intitule_modif }),
            contentType: "application/json; charset=utf-8",
            dataType: "json",

            success: function (data) {
                if (data.ok) {
                    
                    var fd = new FormData();
                    var compteur = 0;
                    var id = id_annonce_clef;

                    var piece = $(this);
                    var ins = document.getElementById('image_modif').files.length;
                    for (var x = 0; x < ins; x++) {
                        fd.append("fileToUpload" + compteur, document.getElementById('image_modif').files[x]);
                        compteur++;
                    }

                    fd.append("id_annonce", id);

                    var ajaxRequest = $.ajax({
                        type: "POST",
                        url: "/Upload/SavePieceJointeAnnonce",
                        async: false,
                        contentType: false,
                        processData: false,
                        data: fd,
                        success: function () {
                        }
                    });
                    
                    Materialize.toast("Les informations sur l'annonce ont été modifiées", 3000);
                    $("#modal_modif_annonce").modal('close');
                    load();
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
        return false;
    });


    
    $(document).on("click", ".supp_pj", function () {
        var id_annonce = $(this).data('num_annonce');
        var id_image = $(this).data('num_image');
        var chemin = $(this).data('chemin');
        $(this).parent("div").remove();
        $.ajax({
            type: "POST",
            url: "/Annonce/DeletePieceJointe",
            data: JSON.stringify({ id_annonce: id_annonce, id_image: id_image, chemin: chemin }),
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            success: function (msg) {
                if (msg.ok) {
                    display_success("fichier supprimé");
                }
                else {
                    display_error(msg.error);
                }
            }
        });
    });
    

})