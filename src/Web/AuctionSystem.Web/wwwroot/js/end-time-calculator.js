let timeInput = $('#end-time-id-for-calculator').val();
let eventTime = moment.utc(timeInput).local();
let currentTime = moment();
let duration = moment.duration(eventTime.diff(currentTime));
let interval = 1000;
let identifier = $('#identifier').val();

setInterval(function () {
        if (identifier === null) {
            return;
        }
        duration = moment.duration(duration - interval, 'milliseconds');
        $('#countdown')
            .text(`${duration.asDays().toFixed()}d ${duration.hours()}h ${duration.minutes()}m ${duration.seconds()}s`);
    },
    interval);