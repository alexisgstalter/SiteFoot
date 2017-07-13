$(document).ready(function () {
   
    
    $('select').material_select();

    loadAllEquipe();

    function loadAllEquipe() {
        $('#requested_equipe').empty();
        $.ajax({
            type: "POST",
            url: "/Backoffice/LoadAllEquipe",
            data: '{}',
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            success: function (msg) {

                if (msg.ok) { //Il n'y a eu aucune erreur côté serveur

                    $('#requested_equipe').append(msg.html);
                    $('#equipetable').DataTable({
                        "scrollY": false,
                        "scrollX": false,
                        "scrollCollapse": false,
                        "searching": true,
                        "ordering": true,
                        "info": true,
                        "paging": true,
                        "order": []
                    });
                    $.each($.fn.dataTable.fnTables(true), function (idx, singleTable) {
                        $(singleTable).dataTable().fnAdjustColumnSizing();
                    });
                    $("select").material_select();

                }
                else {  //Il y eu une erreur côté serveur
                    alert(msg.error);
                    //On enleve le potentiel chargement de la page
                }
            }
        });
    }


    $(document).on('click', '.delete_ligne', function () {
        var ligne = $(this).data("value");
        $.ajax({
            type: "POST",
            url: "/Backoffice/DeleteEquipe",
            data: '{id:"' + ligne + '"}',
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            success: function (data) {

                if (data.ok) { //Il n'y a eu aucune erreur côté serveur
                    Materialize.toast("L'équipe a été supprimée", 3000);
                    loadAllEquipe();
                }
                else {  //Il y eu une erreur côté serveur
                    Materialize.toast(data.error, 3000);
                }
            },
            error: function (xhr, status, error) {
                var err = eval("(" + xhr.responseText + ")");
                alert(err.Message);
            }

        });
    });


    $("#create_equipe").validate({
        errorPlacement: function (error, element) {
            error.insertAfter(element.parent("div"));
        },
        onfocusout: function (element) {
            this.element(element);
        },
        rules: {
            nom_equipe: {
                required: true
            },
            liste_categorie: {
                required: true
            },
            entraineur: {
                required: true
            },
            ecusson: {
                required: true
            }
        },
        messages: {
            nom_equipe: {
                required: "veuillez renseigner ce champs"
            },
            liste_categorie: {
                required: "veuillez renseigner ce champs"
            },
            entraineur: {
                required: "veuillez renseigner ce champs"
            },
            ecusson: {
                required: "veuillez renseigner ce champs"
            }
        },
        submitHandler: function (form) {
            create_info_equipe();
            return false;
        }
    });


    function create_info_equipe() {
        var nom_equipe = $("#nom_equipe").val();
        var liste_categorie = $("#liste_categorie").val();
        var entraineur = Array();
        for (var i = 0; i < $("#entraineur option:selected").length; i++) {
            entraineur.push($("#entraineur option:selected").eq(i).val());
        }
        var ecusson = $("#pj").val();
        $.ajax({
            url: "/Backoffice/SaveEquipe",
            type: "POST",
            data: JSON.stringify({ nom_equipe: nom_equipe, liste_categorie: liste_categorie, entraineur: entraineur, ecusson: ecusson }),
            contentType: "application/json; charset=utf-8",
            dataType:"json",
            success: function (data) {
                if (data.ok) {
                    Materialize.toast("L'équipe a été crée", 3000);
                    loadAllEquipe();
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


    $(document).on("click", ".edit_ligne", function () {
        try {

            var id = $(this).data("id_ligne");

            $.ajax({
                type: "POST",
                url: "/Backoffice/EditEquipe",
                data: JSON.stringify({ id: id }),
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                success: function (data) {
                    if (data.ok) {
                        if (data.hasResult) {
                            $('#EquipeModal').modal('open');
                            $("#equipe_edit").val(data.nom_equipe);
                            $("#categorie_edit").val(data.categorie);
                            $("#entraineur_edit").val(data.id_entraineur);
                            $("#entraineur_edit").material_select();
                            $("#ecusson_edit").val(data.ecusson);
                        }
                    }  
                },
                error: function (xhr, status, error) {
                    var err = eval("(" + xhr.responseText + ")");
                    alert(err.Message);
                }
            });
        }
        catch (e) {
            console.log(e);
            return true;
        }
    });


    $("#EquipeModalForm").submit(function () {
        var nom_equipe_update = $("#equipe_edit").val();
        var categorie_update = $("#categorie_edit").val();
        var entraineur_update = $("#entraineur_edit").val();
        var ecusson_update = $("#ecusson_edit").val();
        $.ajax({
            url: "/Backoffice/UpdateEquipe",
            type: "POST",
            data: { nom_equipe_update: nom_equipe_update, categorie_update: categorie_update, entraineur_update: entraineur_update, ecusson_update: ecusson_update },
            dataType: "json",
            success: function (data) {
                if (data.ok) {
                    Materialize.toast("Les informations sur l'équipe ont été modifiées", 3000);
                    $("#EquipeModal").modal('close');
                    loadAllEquipe();
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


    /*
    $("#OpenUpload").click(function () {
        $('#UploadModal').modal('open');
    });
    */

    $("#upload").click(function () {
        var fd = new FormData();
        var compteur = 0;
       
        var nom_equipe = $("#nom_equipe").val();
        var liste_categorie = $("#liste_categorie").val();
        var entraineur = $("#entraineur").val();
 
        $(".piece_jointe").each(function () {
            var ins = document.getElementById('pj').files.length;
            for (var x = 0; x < ins; x++) {
                fd.append("fileToUpload" + compteur, document.getElementById('pj').files[x]);
                compteur++;
            }
        });
        fd.append("nom_equipe", nom_equipe);
        fd.append("liste_categorie", liste_categorie);
        fd.append("entraineur", entraineur);

        var ajaxRequest = $.ajax({
            type: "POST",
            url: "/Upload/SaveEcussonEquipe",
            async: false,
            contentType: false,
            processData: false,
            data: fd
        });
        find();
    });

})
