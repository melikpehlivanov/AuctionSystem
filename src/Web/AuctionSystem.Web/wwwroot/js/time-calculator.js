let endTime = moment($('#end-time-id-for-calculator').val());
let startTime = moment($('#start-time-id-for-calculator').val());
let currentTime = moment();

let eventTime;
let duration;
if (endTime > currentTime && startTime < currentTime) {
    // Calculate remaining time until auction end
    eventTime = moment.utc(endTime).local();
    duration = moment.duration(eventTime.diff(currentTime));
} else {
    eventTime = moment.utc(startTime).local();
    duration = moment.duration(eventTime.diff(currentTime));
}
let interval = 1000;
let identifier = $('#identifier').val();

setInterval(function () {
    if (identifier === null) {
        return;
    }
    duration = moment.duration(duration - interval, 'milliseconds');
    if (duration < 0) {
        location.reload();
    }
    $('#countdown')
        .text(`${duration.asDays().toFixed()}d ${duration.hours()}h ${duration.minutes()}m ${duration.seconds()}s`);
},
    interval);