$(document).ready(function () {
    $("#adversaire, #adversaire_ajout, #adversaire_modif").autoComplete({
        source: function (term, response) {
            xhr = $.getJSON('/Resultats/AutocompleteAdversaire', { partial_name: term }, function (data) { response(data); });
        },
        onSelect: function (e, term, item) {
            $("#calendrier_entrainement").fullCalendar('refetchEvents');
        }
    });
    $("#add_adv").click(function () {
        $("#modal_ajout_adversaire").modal('open');
    });
    $("#date_debut, #date_fin, #date_ajout, #date_modif").pickadate({
        monthsFull: ['Janvier', 'Février', 'Mars', 'Avril', 'Mai', 'Juin', 'Juillet', 'Août', 'Septembre', 'Octobre', 'Novembre', 'Décembre'],
        monthsShort: ['Janvier', 'Février', 'Mars', 'Avril', 'Mai', 'Juin', 'Juillet', 'Août', 'Septembre', 'Octobre', 'Novembre', 'Décembre'],
        weekdaysShort: ['Dim', 'Lun', 'Mar', 'Mer', 'Jeu', 'Ven', 'Sam'],
        today: 'aujourd\'hui',
        clear: 'effacer',
        formatSubmit: 'dd/mm/yyyy',
        format: "dd/mm/yyyy"
    });
    $("#find_match").validate({
        onfocusout: function (element) {
            this.element(element);
        },
        rules: {
            date_debut: {
                required : true
            },
            date_fin: {
                required : true
            }
        },
        messages: {
            date_debut: {
                required : "veuillez renseigner ce champs"
            },
            date_fin: {
                required: "veuillez renseigner ce champs"
            }
        },
        submitHandler: function () {
            Load();
            return false;
        }
    })
    function Load() {
        var id_equipe = $("#equipe").val();
        var adversaire = $("#adversaire").val();
        var date_debut = $("#date_debut").val();
        var date_fin = $("#date_fin").val();
        $("#resultats_matchs").empty();
        $.ajax({
            url: "GetMatchs",
            type: "POST",
            data: JSON.stringify({id_equipe : id_equipe, adversaire : adversaire, date_debut : date_debut, date_fin : date_fin}),
            dataType: "json",
            contentType: "application/json; charset=utf-8",
            success: function (data) {
                if (data.ok) {
                    $("#resultats_matchs").append(data.html);
                    $(".table").dataTable({

                    });
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
    $("#form_ajout_adversaire").validate({
        errorPlacement: function (error, element) {
            error.insertAfter(element.parent("div"));
        },
        onfocusout: function (element) {
            this.element(element);
        },
        rules: {
            nom: {
                required : true
            },
            categorie: {
                required : true
            }

        },
        messages: {
            nom: {
                required: "Veuillez renseigner ce champs"
            },
            categorie: {
                required: "Veuillez renseigner ce champs"
            }
        },
        submitHandler: function () {
            CreateADV();
            return false;
        }
    });

    function CreateADV() {
        var nom = $("#nom").val();
        var categorie = $("#categorie").val();
        $.ajax({
            url: "SaveAdversaire",
            type: "POST",
            data: JSON.stringify({ nom : nom, categorie : categorie }),
            dataType: "json",
            contentType: "application/json; charset=utf-8",
            success: function (data) {
                if (data.ok) {
                    Materialize.toast("Adversaire ajouté", 3000);
                    var fd = new FormData();
                    var compteur = 0;


                    $(".piece_jointe").each(function () {
                        var ins = document.getElementById('pj').files.length;
                        for (var x = 0; x < ins; x++) {
                            fd.append("fileToUpload" + compteur, document.getElementById('pj').files[x]);
                            compteur++;
                        }
                    });
                    fd.append("id", data.id);

                    var ajaxRequest = $.ajax({
                        type: "POST",
                        url: "/Upload/SaveEcussonAdversaire",
                        async: false,
                        contentType: false,
                        processData: false,
                        data: fd
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
    }
    $("#add_modal").click(function () {
        $("#modal_ajout_match").modal('open');
    });
    var current_id;
    $("#form_ajout_match").validate({
        errorPlacement: function (error, element) {
            error.insertAfter(element.parent("div"));
        },
        onfocusout: function (element) {
            this.element(element);
        },
        rules: {
            adversaire_ajout: {
                required: true
            },
            date_ajout: {
                required: true
            },
            score_equipe_ajout: {
                required: true
            },
            score_adversaire_ajout: {
                required: true
            }
        },
        messages: {
            adversaire_ajout: {
                required: "Veuillez renseigner ce champs"
            },
            date_ajout: {
                required: "Veuillez renseigner ce champs"
            },
            score_equipe_ajout: {
                required: "Veuillez renseigner ce champs"
            },
            score_adversaire_ajout: {
                required: "Veuillez renseigner ce champs"
            }
        },
        submitHandler: function () {
            SaveMatch();
            return false;
        }
    });
    function SaveMatch() {
        var id_equipe = $("#equipe_ajout").val();
        var adversaire = $("#adversaire_ajout").val();
        var date = $("#date_ajout").val();
        var score_equipe = $("#score_equipe_ajout").val();
        var score_adversaire = $("#score_adversaire_ajout").val();
        $("#resultats_matchs").empty();
        $.ajax({
            url: "SaveMatch",
            type: "POST",
            data: JSON.stringify({ id_equipe: id_equipe, adversaire: adversaire, date : date, score_equipe : score_equipe, score_adversaire : score_adversaire}),
            dataType: "json",
            contentType: "application/json; charset=utf-8",
            success: function (data) {
                if (data.ok) {
                    Materialize.toast("Match enregistré", 3000);
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
    $("#btn_supprimer").click(function () {
        var nom = $("#nom").val();
        var categorie = $("#categorie").val();
        $.ajax({
            url: "DeleteAdversaire",
            type: "POST",
            data: JSON.stringify({ nom: nom, categorie: categorie }),
            dataType: "json",
            contentType: "application/json; charset=utf-8",
            success: function (data) {
                if (data.ok) {
                    Materialize.toast("Adversaire supprimé", 3000);
                    if ($("#date_debut").val() != "") {
                        Load();
                    }
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
    $("#form_modif_match").validate({
        errorPlacement: function (error, element) {
            error.insertAfter(element.parent("div"));
        },
        onfocusout: function (element) {
            this.element(element);
        },
        rules: {
            adversaire_modif: {
                required: true
            },
            date_modif: {
                required: true
            },
            score_equipe_modif: {
                required: true
            },
            score_adversaire_modif: {
                required: true
            }
        },
        messages: {
            adversaire_modif: {
                required: "Veuillez renseigner ce champs"
            },
            date_modif: {
                required: "Veuillez renseigner ce champs"
            },
            score_equipe_modif: {
                required: "Veuillez renseigner ce champs"
            },
            score_adversaire_modif: {
                required: "Veuillez renseigner ce champs"
            }
        },
        submitHandler: function () {
            UpdateMatch();
            return false;
        }
    });
    $(document).on('click', '.edit', function () {
        current_id = $(this).data("value");
        var parent = $(this).parent("tr");
        $("#equipe_modif option").each(function () {
            if ($(this).text() == parent.find("td").eq(0).text().trim()) {
                $(this).prop("selected", true);
            }
            else {
                $(this).prop("selected", false);
            }
        });
        $("select").material_select();
        $("#adversaire_modif").val(parent.find("td").eq(1).text().trim());

        $("#score_equipe_modif").val(parent.find("td").eq(2).text().split('-')[0].trim());
        $("#score_adversaire_modif").val(parent.find("td").eq(2).text().split('-')[1].trim());
        $("#date_modif").val(parent.find("td").eq(3).text());
        Materialize.updateTextFields();
        $("#modal_modif_match").modal('open');
    });
    function UpdateMatch() {
        var id_equipe = $("#equipe_modif").val();
        var adversaire = $("#adversaire_modif").val();
        var date = $("#date_modif").val();
        var score_equipe = $("#score_equipe_modif").val();
        var score_adversaire = $("#score_adversaire_modif").val();
        $.ajax({
            url: "UpdateMatch",
            type: "POST",
            data: JSON.stringify({id : current_id, id_equipe: id_equipe, adversaire: adversaire, date: date, score_equipe: score_equipe, score_adversaire: score_adversaire }),
            dataType: "json",
            contentType: "application/json; charset=utf-8",
            success: function (data) {
                if (data.ok) {
                    Materialize.toast("Match modifié", 3000);
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
        current_id = $(this).data("value");
        $.ajax({
            url: "DeleteMatch",
            type: "POST",
            data: JSON.stringify({ id : current_id }),
            dataType: "json",
            contentType: "application/json; charset=utf-8",
            success: function (data) {
                if (data.ok) {
                    Materialize.toast("Match supprimé", 3000);
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
});