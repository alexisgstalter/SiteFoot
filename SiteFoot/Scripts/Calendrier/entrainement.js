$(document).ready(function () {
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
                url: "/Calendrier/GetEventBuvette",
                type: "POST",
                data: { id: calEvent.id },
                dataType: "json",
                success: function (data) {
                    if (data.ok) {
                        if (data.hasResult) {
                            $("#responsable").val(data.responsable);
                            $("#responsable").material_select();
                            $("#intitule").val(data.titre);
                            $("#debut").val(data.heure_debut);
                            $("#fin").val(data.heure_fin);
                            Materialize.updateTextFields();
                            $("#modal_modif_buvette").modal('open');
                            current_id = data.id;
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
})