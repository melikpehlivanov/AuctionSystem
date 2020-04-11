(function () {
    const euroSign = '€';
    const currentUserId = $('#currentUserId').val();
    const consoleId = $('#consoleId').val();

    const startingPrice = parseFloat($('#startingPrice').val());
    const minIncrease = parseFloat($('#minIncrease').val());

    const bidInput = $('#bid-amount');
    const highestBidInput = $('#highestBid');
    const bidButton = $('#button-bid');

    (function () {
        let connection =
            new signalR.HubConnectionBuilder()
                .withUrl('/bidHub')
                .build();

        connection.start()
            .then(function () {
                connection.invoke('Setup', consoleId);
            })
            .catch(function (err) {
                return console.error(err.toString());
            });

        connection.on('ReceivedMessage',
            function (bidAmount, userId) {
                let highestBid = parseFloat(highestBidInput.val());

                if (bidAmount > highestBid) {
                    changeCurrentPriceStatus(bidAmount);
                    updateSuggestedBid();
                }


                let li;
                if (currentUserId === userId) {
                    li = $('<li>')
                        .append($('<div>')
                            .addClass('yellow-message')
                            .text(`You've successfully bid ${euroSign}${bidAmount.toFixed(2)}`));
                } else {
                    li = $('<li>')
                        .append($('<div>')
                            .addClass('message')
                            .text(`${euroSign}${bidAmount.toFixed(2)}: Competing Bid`));
                }

                let messageArea = $('#chat-messages');

                li.hide().appendTo(messageArea).fadeIn(350);

                let chat = document.getElementsByClassName('chat');
                chat[0].scrollTop = chat[0].scrollHeight;
            });

        function changeCurrentPriceStatus(currentBidAmount) {

            highestBidInput.attr('value', currentBidAmount);

            let digits = (euroSign + currentBidAmount.toFixed(2)).split('');

            let priceStorage = $('#price-storage');
            priceStorage.empty();

            digits.forEach(digit => {
                let div = $('<div>')
                    .addClass('custom-price-card')
                    .append($('<span>')
                        .addClass('text-white')
                        .text(digit.toString()));

                priceStorage.append(div);
            });
        }

        function updateSuggestedBid() {
            $('#suggested-bid-button').text(`Bid ${euroSign}${getMinBid().toFixed(2)}`);
        }

        function getMinBid() {
            let highestBid = parseFloat(highestBidInput.val());
            return highestBid === 0 ? startingPrice : highestBid + minIncrease;
        }

        bidButton.click(function () {
            let enteredBid = bidInput.val();

            if (!enteredBid) {
                $(bidInput).notify('Please enter bidding amount');
                return;
            }

            let bidAmount = parseFloat(enteredBid);

            let minBid = getMinBid();

            if (bidAmount < minBid) {
                $(bidInput).notify(`You have to bid at least ${euroSign}${minBid.toFixed(2)}`);
                return;
            }

            bidInput.val('');

            connection.invoke('CreateBidAsync', bidAmount, consoleId);
        });

        $('#suggested-bid-button').click(function () {
            connection.invoke('CreateBidAsync', getMinBid(), consoleId);
        });
    })();

})();