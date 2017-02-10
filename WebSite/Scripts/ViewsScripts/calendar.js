function Calendar() {
    var jsonData;

    function loadCalendar() {
        $('#calendar').fullCalendar({
            header: {
                left: 'prev,next today',
                center: 'title',
                right: 'month,agendaWeek,agendaDay'
            },
            defaultView: 'agendaDay',
            navLinks: true,
            editable: false,
            minTime: "07:00:00",
            maxTime: "22:00:00",
            timeFormat: 'H:mm',
            events: {
                url: '/ScheduleData/ScheduleCalendar',
                borderColor: "#4682B4",
                success: function (events) {
                    jsonData = events;
                },
                error: function () {
                    alert("Calendar Error!");
                }
            },
            viewRender: function (view) {
                if (typeof (timelineInterval) != 'undefined') {
                    window.clearInterval(timelineInterval);
                }
                timelineInterval = window.setInterval(setTimeline, 30000);
                try {
                    setTimeline();
                } catch (err) { }
                postEvent();
            },
            eventAfterAllRender: function (view) {
                if (jsonData !== undefined && view.intervalUnit == "day")
                    DisplayInfo();
            }
        });
    }

    function postEvent() {
        $("#calendar").fullCalendar("refetchEvents");
    }

    function setTimeline(view) {
        var parentDiv = $('.fc-slats:visible').parent();
        var timeline = parentDiv.children(".timeline");
        if (timeline.length == 0) { //if timeline isn't there, add it
            timeline = $("<hr>").addClass("timeline").css("z-index", "99999");
            parentDiv.prepend(timeline);
        }

        var curTime = new Date();

        var curCalView = $("#calendar").fullCalendar('getView');
        if (curCalView.intervalStart < curTime && curCalView.intervalEnd > curTime) {
            timeline.show();
        } else {
            timeline.hide();
            return;
        }

        if (curCalView.name !== "month") {
            var calMinTimeInMinutes = strTimeToMinutes(curCalView.opt("minTime"));
            var calMaxTimeInMinutes = strTimeToMinutes(curCalView.opt("maxTime"));
            var curSeconds = ((((curTime.getHours() * 60) + curTime.getMinutes()) - calMinTimeInMinutes) * 60) + curTime.getSeconds();
            var percentOfDay = curSeconds / ((calMaxTimeInMinutes - calMinTimeInMinutes) * 60);

            var topLoc = Math.floor(parentDiv.height() * percentOfDay);
            var timeCol = $('.fc-time:visible');
            timeline.css({ top: topLoc + "px", left: ((timeCol.outerWidth(true)) - 20) + "px" });

            if (curCalView.name == "agendaWeek") { //week view, don't want the timeline to go the whole way across
                var dayCol = $(".fc-today:visible");
                var left = dayCol.position().left + 1;
                var width = dayCol.width() + 1;
                timeline.css({ left: left + "px", width: width + "px" });
            }
        }
    }

    function strTimeToMinutes(str_time) {
        var arr_time = str_time.split(":");
        var hour = parseInt(arr_time[0]);
        var minutes = parseInt(arr_time[1]);
        return ((hour * 60) + minutes);
    }
    function submitReportHandler() {
        $(".downloadBtn").onclick = function () {
            var fileID = $(this).attr('value');
            download(fileID);
        }
    }

    function download(fileId) {
        $.ajax({
            url: "/ScheduleData/Download",
            contentType: "application/json; charset=utf-8",
            type: 'GET',
            data: { eventID: fileId },
            dataType: "json",
            success: function (data) {
                if (data) {
                    bootbox.alert('The file was downloaded successfully :)');
                }
            },
            error: function (data) {
                bootbox.alert('The file was not downloaded successfully :(');
            },
            fail: function (data) {
                bootbox.alert('The file was not downloaded successfully :(');
            }
        });

    }

    function DisplayInfo() {
        $.each(jsonData, function (i, item) {
            var rows = "";
            var depStat = "";
            var reqStat = "";
            var succNum = 0;
            var unSuccNum = 0;
            var froStat = 0;
            var condition = "";

            $(".fc-content").each(function () {
                if ($(".fc-title", $(this)).text() == item.title && item.FilePath != null) {
                    var id = item.id;
                    $(this)
                    .prepend(
                        $("<span>")
                            .append($('<span data-toggle="tooltip" data-placement="bottom" title="Details : ' + item.title + '">').addClass("glyphicon glyphicon-info-sign btn btn-sxs btn-info btnEdit"))
                            .css("margin-top", "2px").css("margin-right", "4px").css("float", "right")
                            .click(function () {
                                bootbox.dialog({
                                    title: "Discipline: " + item.title + "",
                                    message:
                                        "<span><b>Type:</b></span><pre>" + item.Type + "</pre>" +
                                        "<span><b>Topic:</b></span><pre>" + item.Topic + "</pre>" +
                                        "<span><b>Room:</b></span><pre>" + item.Room + "</pre>" +
                                        "<span><b>Teacher:</b></span><pre>" + item.TeacherName + "</pre>" +
                                        "<span><b>Start Time:</b></span><pre>" + item.start + "</pre>" +
                                         "<span><b>End Time:</b></span><pre>" + item.end + "</pre>" +
                                        "<span><b>File:</b></span><pre>" + item.FilePath + "</pre>",
                                    //onclick='download(" + id + ")
                                    className: "shedulerClass",

                                    buttons: {
                                        download: {
                                            label: "Download", className: "btn-default Download pull-left", callback: function () {
                                                download(item.ID);
                                            }
                                        },
                                        success: {
                                            label: "Teacher profile", className: "btn-success btn-edit", callback: function () { return dsUtil.onEditClick(item); }
                                        }
                                    }
                                });

                            })
                            )
                };
            });
        });
    }
    return {
        initCalendar: function () {
            loadCalendar();
            submitReportHandler()
        },
        download: function (fileID) {
            download(fileID)
        }
    }
}