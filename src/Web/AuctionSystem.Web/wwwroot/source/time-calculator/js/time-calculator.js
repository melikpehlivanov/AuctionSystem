(function () {
    const interval = 1000;

    let endTime = moment.utc($('#end-time-id-for-calculator').val());
    let startTime = moment.utc($('#start-time-id-for-calculator').val());
    let currentTime = moment();

    let duration;
    if (startTime > currentTime) {
        // Calculate remaining time until auction start
        let eventTime = moment.utc(startTime).local();
        duration = moment.duration(eventTime.diff(currentTime));
    } else {
        let eventTime = moment.utc(moment(endTime)).local();
        duration = moment.duration(eventTime.diff(currentTime));
    }

    let identifier = $('#identifier').val();

    setInterval(function () {
            if (identifier === null) {
                return;
            }
            duration = moment.duration(duration - interval, 'milliseconds');
            if (duration < 0 && endTime > currentTime) {
                location.reload();
            }

            $('#countdown')
                .text(`${duration.asDays().toFixed()}d ${duration.hours()}h ${duration.minutes()}m ${duration.seconds()}s`);
        },
        interval);
})();