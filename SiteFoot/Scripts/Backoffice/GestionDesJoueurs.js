$(document).ready(function () {
    Load();
    function Load() {
        $("#membre_container").empty();
        $.ajax({
            url: "GetJoueurs",
            type: "POST",
            data: JSON.stringify({ }),
            dataType: "json",
            contentType: "application/json; charset=utf-8",
            success: function (data) {
                if (data.ok) {
                    $("#membre_container").append(data.html);
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
        $("#modal_ajout_membre").modal('open');
    });
    $("#form_ajout_membre").validate({
        errorPlacement: function (error, element) {
            error.insertAfter(element.parent("div"));
        },
        onfocusout: function (element) {
            this.element(element);
        },
        rules: {
            nom_ajout: {
                required: true
            },
            prenom_ajout: {
                required: true
            },
            telephone_ajout: {
                required: true,
            }
        },
        messages: {
            nom_ajout: {
                required: "veuillez renseigner ce champs"
            },
            prenom_ajout: {
                required: "veuillez renseigner ce champs"
            },
            telephone_ajout: {
                required: "veuillez renseigner ce champs"
            }
        },
        submitHandler: function (form) {
            Create();
            return false;
        }
    });
    $("#form_modif_membre").validate({
        errorPlacement: function (error, element) {
            error.insertAfter(element.parent("div"));
        },
        onfocusout: function (element) {
            this.element(element);
        },
        rules: {
            nom_modif: {
                required: true
            },
            prenom_modif: {
                required: true
            },
            telephone_modif: {
                required: true,
            }
        },
        messages: {
            nom_modif: {
                required: "veuillez renseigner ce champs"
            },
            prenom_modif: {
                required: "veuillez renseigner ce champs"
            },
            telephone_modif: {
                required: "veuillez renseigner ce champs"
            }
        },
        submitHandler: function (form) {
            Update();
            return false;
        }
    });
    function Create() {
        var id_equipe = $("#equipe_ajout").val();
        var nom = $("#nom_ajout").val();
        var prenom = $("#prenom_ajout").val();
        var adresse = $("#adresse_ajout").val();
        var telephone = $("#telephone_ajout").val();
        var email = $("#email_ajout").val();
        $.ajax({
            url: "SaveJoueur",
            type: "POST",
            data: JSON.stringify({id_equipe : id_equipe, nom : nom, prenom : prenom, adresse : adresse, telephone : telephone, email : email}),
            dataType: "json",
            contentType: "application/json; charset=utf-8",
            success: function (data) {
                if (data.ok) {
                    Materialize.toast("Joueur créé", 3000);
                    Load();
                }
                else {
                    Materialize.toast(data.error, 3000);
                }


            },
            error: function (xhr, status, error) {
            }
        });
    }
    
    function Update() {
        var id_equipe = $("#equipe_modif").val();
        var nom = $("#nom_modif").val();
        var prenom = $("#prenom_modif").val();
        var adresse = $("#adresse_modif").val();
        var telephone = $("#telephone_modif").val();
        var email = $("#email_modif").val();
        $.ajax({
            url: "UpdateJoueur",
            type: "POST",
            data: JSON.stringify({id : current_id, id_equipe: id_equipe, nom: nom, prenom: prenom, adresse: adresse, telephone: telephone, email: email }),
            dataType: "json",
            contentType: "application/json; charset=utf-8",
            success: function (data) {
                if (data.ok) {
                    Materialize.toast("Joueur modifié", 3000);
                    Load();
                }
                else {
                    Materialize.toast(data.error, 3000);
                }


            },
            error: function (xhr, status, error) {
            }
        });
    }
    $(document).on('click', '.supp', function () {
        var id = $(this).data("value");
        $.ajax({
            url: "DeleteJoueur",
            type: "POST",
            data: JSON.stringify({ id : id}),
            dataType: "json",
            contentType: "application/json; charset=utf-8",
            success: function (data) {
                if (data.ok) {
                    Materialize.toast("Joueur supprimé", 3000);
                    Load();
                }
                else {
                    Materialize.toast(data.error, 3000);
                }


            },
            error: function (xhr, status, error) {
            }
        });
    });
    var current_id;

    $(document).on('click', '.edit', function () {
        current_id = $(this).data("value");
        var equipe = $(this).parent("tr").find("td").eq(5).text().trim();
        $("#equipe_modif option").each(function () {
            if ($(this).text() == equipe) {
                $(this).prop("selected", "selected");
            }
        });
        $("select").material_select();
        $("#prenom_modif").val($(this).parent("tr").find("td").eq(0).text());
        $("#nom_modif").val($(this).parent("tr").find("td").eq(1).text());
        $("#adresse_modif").val($(this).parent("tr").find("td").eq(2).text());
        $("#telephone_modif").val($(this).parent("tr").find("td").eq(3).text());
        $("#email_modif").val($(this).parent("tr").find("td").eq(4).text());
        Materialize.updateTextFields();
        $("#modal_modif_membre").modal('open');
    });
});