$(document).ready(function () {
   
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
                    Materialize.toast("L'équipe a été supprimé", 3000);
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
        var entraineur = $("#entraineur").val();
        var ecusson = $("#ecusson").val();
        $.ajax({
            url: "/Backoffice/SaveEquipe",
            type: "POST",
            data: { nom_equipe: nom_equipe, liste_categorie: liste_categorie, entraineur: entraineur, ecusson: ecusson },
            dataType: "json",
            success: function (data) {
                if (data.ok) {
                    Materialize.toast("L'équipe a été créé", 3000);
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


})
