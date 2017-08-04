$(document).ready(function () {
    var current_id;
    Load();
    function Load() {
        $("#groupes_container").empty();
        $.ajax({
            url: "GetAllGroupes",
            type: "POST",
            data: JSON.stringify({}),
            dataType: "json",
            contentType: "application/json; charset=utf-8",
            success: function (data) {
                if (data.ok) {
                    $("#groupes_container").append(data.html);
                    $(".table").dataTable();
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
        $("#modal_ajout_groupe").modal('open');
    });

    $("#form_ajout_event").validate({
        rules: {
            nom_ajout: {
                required : true
            }
        },
        messages: {
            nom_ajout: {
                required : "veuillez renseigner ce champs"
            }
        },
        submitHandler: function () {
            Create();
            return false;
        }
    });
    function Create() {
        var droit_gerer_buvette = $("#droit_gerer_buvette").is(":checked");
        var droit_gerer_entrainement = $("#droit_gerer_entrainement").is(":checked");
        var droit_gerer_formateur = $("#droit_gerer_formation").is(":checked");
        var droit_formateur_autres = $("#droit_formation_autres").is(":checked");
        var droit_entrainement_autres = $("#droit_entrainement_autres").is(":checked");
        var droit_poster_annonce = $("#droit_poster_annonce").is(":checked");
        var nom = $("#nom_ajout").val();

        $.ajax({
            url: "SaveGroupe",
            type: "POST",
            data: JSON.stringify({ nom : nom, droit_gerer_buvette : droit_gerer_buvette, droit_gerer_entrainement : droit_gerer_entrainement, droit_entrainement_autres : droit_entrainement_autres, droit_gerer_formateur : droit_gerer_formateur, droit_formateur_autres : droit_formateur_autres, droit_poster_annonce : droit_poster_annonce }),
            dataType: "json",
            contentType: "application/json; charset=utf-8",
            success: function (data) {
                if (data.ok) {
                    Materialize.toast("Groupe créé", 3000);
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
    }

    $(document).on('click', '.supp', function () {
        var id = $(this).data("value");
        $.ajax({
            url: "DeleteGroupe",
            type: "POST",
            data: JSON.stringify({ id : id }),
            dataType: "json",
            contentType: "application/json; charset=utf-8",
            success: function (data) {
                if (data.ok) {
                    Materialize.toast("Groupe supprimé", 3000);
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
    $(document).on('click', '.edit', function () {
        current_id = $(this).data("value");
        $("#nom_modif").val($(this).parent("tr").find("td").eq(0).text());
        if ($(this).parent("tr").find("td").eq(1).text() == "1") {
            $("#droit_gerer_buvette_modif").prop("checked", true);
        }
        else {
            $("#droit_gerer_buvette_modif").prop("checked", false);
        }
        if ($(this).parent("tr").find("td").eq(2).text() == "1") {
            $("#droit_gerer_entrainement_modif").prop("checked", true);
        }
        else{
            $("#droit_gerer_entrainement_modif").prop("checked", false);
        }
        if ($(this).parent("tr").find("td").eq(3).text() == "1") {
            $("#droit_entrainement_autres_modif").prop("checked", true);
        }
        else {
            $("#droit_entrainement_autres_modif").prop("checked", false);
        }
        if ($(this).parent("tr").find("td").eq(4).text() == "1") {
            $("#droit_gerer_formation_modif").prop("checked", true);
        }
        else {
            $("#droit_gerer_formation_modif").prop("checked", false);
        }
        if ($(this).parent("tr").find("td").eq(5).text() == "1") {
            $("#droit_formation_autres_modif").prop("checked", true);
        }
        else {
            $("#droit_formation_autres_modif").prop("checked", false);
        }
        if ($(this).parent("tr").find("td").eq(6).text() == "1") {
            $("#droit_poster_annonce_modif").prop("checked", true);
        }
        else {
            $("#droit_poster_annonce_modif").prop("checked", false);
        }
        Materialize.updateTextFields();
        $("#modal_modif_groupe").modal('open');
    });

    $("#form_modif_event").submit(function () {
        var droit_gerer_buvette = $("#droit_gerer_buvette_modif").is(":checked");
        var droit_gerer_entrainement = $("#droit_gerer_entrainement_modif").is(":checked");
        var droit_gerer_formateur = $("#droit_gerer_formation_modif").is(":checked");
        var droit_formateur_autres = $("#droit_formation_autres_modif").is(":checked");
        var droit_entrainement_autres = $("#droit_entrainement_autres_modif").is(":checked");
        var droit_poster_annonce = $("#droit_poster_annonce_modif").is(":checked");
        var nom = $("#nom_modif").val();

        $.ajax({
            url: "UpdateGroupe",
            type: "POST",
            data: JSON.stringify({ id : current_id, nom: nom, droit_gerer_buvette: droit_gerer_buvette, droit_gerer_entrainement: droit_gerer_entrainement, droit_entrainement_autres: droit_entrainement_autres, droit_gerer_formateur: droit_gerer_formateur, droit_formateur_autres: droit_formateur_autres, droit_poster_annonce: droit_poster_annonce }),
            dataType: "json",
            contentType: "application/json; charset=utf-8",
            success: function (data) {
                if (data.ok) {
                    Materialize.toast("Groupe modifié", 3000);
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
        return false;
    });
});