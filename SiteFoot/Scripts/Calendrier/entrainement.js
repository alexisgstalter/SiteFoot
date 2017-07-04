$(document).ready(function () {
    var current_id;
    $("#calendrier_entrainement").fullCalendar({
        header: {
            left: 'prev,next today',
            center: 'title',
            right: 'month,agendaWeek,agendaDay',
            lang:'fr'
        },
        allDayDefault : false,
        startParam: 'start',
        endParam : 'end',
        eventSources: [
            {
                url: "/Calendrier/GetEventsEntrainement",
                type: "POST",
                data: {},
                dataType : "json",
                success: function (data) {
                    
                },
                error: function (xhr, status, error) {
                    var err = eval("(" + xhr.responseText + ")");
                    alert(err.Message);
                }
            }
        ],
        timeFormat: 'H:mm',
        eventClick: function (calEvent, jsEvent, view) {
            $("#equipe").empty();
            $("#terrain").empty();
            $("#debut").empty();
            $("#fin").empty();
            $.ajax({
                url: "/Calendrier/GetEntrainement",
                type: "POST",
                data: {id : calEvent.id},
                dataType: "json",
                success: function (data) {
                    if (data.ok) {
                        if (data.hasResult) {
                            $("#equipe").val(data.equipe);
                            $("#terrain").val(data.terrain);
                            $("#debut").val(data.heure_debut);
                            $("#fin").val(data.heure_fin);
                            Materialize.updateTextFields();
                            $("#modal_modif_entrainement").modal('open');
                            current_id = data.id;
                        }
                    }
                },
                error: function (xhr, status, error) {
                    var err = eval("(" + xhr.responseText + ")");
                    alert(err.Message);
                }
            });

        }
    });
})
