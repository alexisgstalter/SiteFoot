$(document).ready(function () {
    $("#debut ,#fin ,#debut_modif ,#fin_modif,#debut_ajout ,#fin_ajout").datetimepicker({
        lang: 'fr',
        mask: true,
        format: 'd/m/Y H:i',
        step : 15
    });
    $("#membre").autoComplete({
        source: function (term, response) {
            xhr = $.getJSON('/Calendrier/AutocompleteMembre', { partial_name: term }, function (data) { response(data); });
        },
        onSelect: function (e, term, item) {
            $("#calendrier_entrainement").fullCalendar('refetchEvents');
        }
    });
    $("select").change(function () {
        $("#calendrier_entrainement").fullCalendar('refetchEvents');
    });
    var current_id;
    $("#calendrier_entrainement").fullCalendar({
        header: {
            left: 'prev,next today',
            center: 'title',
            right: 'month,agendaWeek,agendaDay',
            lang: 'fr'
        },
        allDayDefault: false,
        startParam: 'start',
        endParam: 'end',
        eventSources: [
            {
                url: "/Calendrier/GetEventsEntrainement",
                type: "POST",
                data: function(){
                    return {
                        id_entraineur: $("#entraineur").val(),
                        id_equipe: $("#equipe").val(),
                        id_membre: $("#membre").val(),
                        id_terrain: $("#terrain").val()
                    };
                },
                dataType: "json",
                success: function (data) {
                },
                error: function (xhr, status, error) {
                    alert(error + " " + status);
                }
            }
        ],
        timeFormat: 'H:mm',
        eventClick: function (calEvent, jsEvent, view) {
            $("#intitule").empty();
            $("#debut").empty();
            $("#fin").empty();
            $.ajax({
                url: "/Calendrier/GetEventEntrainement",
                type: "POST",
                data: { id: calEvent.id },
                dataType: "json",
                success: function (data) {
                    if (data.ok) {
                        if (data.hasResult) {
                            $("#equipe_modif").val(data.equipe);
                            $("#equipe_modif").material_select();
                            $("#terrain_modif").val(data.terrain);
                            $("#terrain_modif").material_select();
                            $("#intitule_modif").val(data.titre);
                            $("#debut_modif").val(data.heure_debut);
                            $("#fin_modif").val(data.heure_fin);
                            Materialize.updateTextFields();
                            $("#modal_modif_entrainement").modal('open');
                            current_id = data.id;
                            $.ajax({
                                url: "/Calendrier/CheckEntraineurEquipe",
                                type: "POST",
                                data: { id_equipe : $("#equipe_modif").val() },
                                dataType: "json",
                                success: function (data) {
                                    if (data) {
                                        $("#btn_modifier").prop("disabled", false);
                                        $("#btn_supprimer").prop("disabled", false);
                                    }
                                    else {
                                        $("#btn_modifier").prop("disabled", true);
                                        $("#btn_supprimer").prop("disabled", true);
                                    }
                                    $("#modal_modif_entrainement").modal('open');

                                },
                                error: function (xhr, status, error) {
                                    var err = eval("(" + xhr.responseText + ")");
                                    alert(err.Message);
                                }
                            });
                        }
                    }
                },
                error: function (xhr, status, error) {
                    var err = eval("(" + xhr.responseText + ")");
                    alert(err.Message);
                }
            });

        },
        displayEventEnd: true
    });
    $("#add_modal").click(function () {
        $("#modal_ajout_entrainement").modal('open');
    });
    $("#form_modif_event").validate({
        errorPlacement: function (error, element) {
            error.insertBefore(element.parent("div"));
        },
        onfocusout: function (element) {
            this.element(element);
        },
        rules: {
            terrain_modif: {
                required: true
            },
            equipe_modif: {
                required: true,
                type: "post",
                remote : "/Calendrier/CheckEntraineurEquipe",
                data: {
                    id_equipe: function () {
                        return $("#equipe_modif").val();
                    }
                    
                }
            },
            intitule_modif: {
                required: true
            },
            debut_modif: {
                required: true
            },
            fin_modif: {
                required: true
            }

        },
        messages: {
            terrain_modif: {
                required: "veuillez renseigner ce champs"
            },
            intitule_modif: {
                required: "veuillez renseigner ce champs"
            },
            equipe_modif: {
                required: "veuillez renseigner ce champs",
                remote : "Vous n'êtes pas l'entraineur de cette équipe"
            },
            debut_modif: {
                required: "veuillez renseigner ce champs"
            },
            fin_modif: {
                required: "veuillez renseigner ce champs"
            }
        },
        submitHandler: function (form) {
            Update();
            return false;
        }
    });
    function Update() {
        var id_equipe = $("#equipe_modif").val();
        var id_terrain = $("#terrain_modif").val();
        var title = $("#intitule_modif").val();
        var start = $("#debut_modif").val();
        var end = $("#fin_modif").val();
        $.ajax({
            url: "/Calendrier/UpdateEntrainement",
            type: "POST",
            data: { id: current_id, title: title, start : start, end : end, id_terrain: id_terrain, id_equipe: id_equipe },
            dataType: "json",
            success: function (data) {
                if (data.ok) {
                    Materialize.toast("l'entrainement a été modifié",3000);
                    $("#modal_modif_entrainement").modal('close');
                    $("#calendrier_entrainement").fullCalendar('refetchEvents');
                }
                else {
                    Materialize.toast(data.error,3000);
                }
                

            },
            error: function (xhr, status, error) {
                var err = eval("(" + xhr.responseText + ")");
                alert(err.Message);
            }
        });
    }
    $("#form_ajout_event").validate({
        onfocusout: function (element) {
            this.element(element);
        },
        rules: {
            terrain_ajout: {
                required: true
            },
            equipe_ajout: {
                required: true,
                type: "post",
                remote: "/Calendrier/CheckEntraineurEquipe",
                data: {
                    id_equipe: function () {
                        return $("#equipe_ajout").val();
                    }

                }
            },
            intitule_ajout: {
                required: true
            },
            debut_ajout: {
                required: true
            },
            fin_ajout: {
                required: true
            }

        },
        messages: {
            terrain_ajout: {
                required: "veuillez renseigner ce champs"
            },
            intitule_ajout: {
                required: "veuillez renseigner ce champs"
            },
            equipe_ajout: {
                required: "veuillez renseigner ce champs",
                remote: "Vous n'êtes pas l'entraineur de cette équipe"
            },
            debut_ajout: {
                required: "veuillez renseigner ce champs"
            },
            fin_ajout: {
                required: "veuillez renseigner ce champs"
            }
        },
        submitHandler: function (form) {
            Create();
            return false;
        }
    });

    function Create() {
        var id_equipe = $("#equipe_ajout").val();
        var id_terrain = $("#terrain_ajout").val();
        var title = $("#intitule_ajout").val();
        var start = $("#debut_ajout").val();
        var end = $("#fin_ajout").val();
        $.ajax({
            url: "/Calendrier/SaveEntrainement",
            type: "POST",
            data: { title: title, start: start, end: end, id_terrain: id_terrain, id_equipe: id_equipe },
            dataType: "json",
            success: function (data) {
                if (data.ok) {
                    Materialize.toast("l'entrainement a été créé", 3000);
                    $("#modal_ajout_entrainement").modal('close');
                    $("#calendrier_entrainement").fullCalendar('refetchEvents');
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
        $.ajax({
            url: "/Calendrier/DeleteEntrainement",
            type: "POST",
            data: { id:current_id },
            dataType: "json",
            success: function (data) {
                if (data.ok) {
                    Materialize.toast("l'entrainement a été supprimé", 3000);
                    $("#modal_modif_entrainement").modal('close');
                    $("#calendrier_entrainement").fullCalendar('refetchEvents');
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
})