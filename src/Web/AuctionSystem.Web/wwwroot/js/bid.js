const euroSign = '€';
const currentUserId = $('#currentUserId').val();
const consoleId = $('#consoleId').val();

const bidInput = document.getElementById('bid-amount');
const highestBidInput = $('#highestBid');

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
            let nextValue = (bidAmount + (bidAmount * 0.1)).toFixed(2);
            changeCurrentBidValueOnTenPercentHigherBidButton(nextValue);
        }

        changeCurrentPriceStatus(bidAmount);

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

        messageArea.append(li).fadeIn(350);

        let chat = document.getElementsByClassName("chat");
        chat[0].scrollTop = chat[0].scrollHeight;
    });

function changeCurrentPriceStatus(currentBidAmount) {
    let highestBid = highestBidInput.val();

    if (currentBidAmount < highestBid) {
        return;
    }
    if (highestBid < currentBidAmount) {
        highestBidInput.attr('value', currentBidAmount);
    }

    let digits = currentBidAmount.toFixed(2).toString().split('');

    let priceStorage = $('#price-storage');
    priceStorage.empty();

    let currencySign = $('<div>')
        .addClass('custom-price-card')
        .append($('<span>')
            .addClass('text-white')
            .text(euroSign));

    priceStorage.append(currencySign);
    digits.forEach(digit => {
        let div = $('<div>')
            .addClass('custom-price-card')
            .append($('<span>')
                .addClass('text-white')
                .text(digit.toString()));

        priceStorage.append(div);
    });
}

function changeCurrentBidValueOnTenPercentHigherBidButton(amount) {
    $('#bid-10-percent-higher-button').text(`Bid: 10% higher (${amount})`);
}

function createBid() {
    let bidAmount = bidInput.value;
    let bidMinAttribute = bidInput.min;
    let parsedBidInput = parseFloat(highestBidInput.val());

    if (!bidAmount) {
        $('#bid-amount').notify('Please enter some value in order to bid');
        return;
    }
    if (parsedBidInput === 0 && bidAmount < bidMinAttribute) {
        $('#bid-amount').notify(`Minimum bid amount ${bidMinAttribute}`);
        return;
    }
    if (bidMinAttribute > bidAmount || bidAmount < parsedBidInput) {
        $('#bid-amount').notify('You cannot bid lower amount than the current highest bid');
        return;
    }

    connection.invoke('CreateBidAsync', bidAmount, consoleId);
}

function createTenPercentHigherBid() {
    let highestBid = parseFloat(highestBidInput.val());
    let bidMinAttribute = parseFloat(bidInput.min);
    let amount;
    if (highestBid == 0) {
        amount = bidMinAttribute + bidMinAttribute * 0.1;
    } else {
        amount = highestBid + highestBid * 0.1;
    }

    connection.invoke('CreateBidAsync', amount, consoleId);
}
