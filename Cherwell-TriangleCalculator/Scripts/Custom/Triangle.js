/// <reference path="../mvc4/jquery-1.10.2.js" />

var Triangle = function () {
    console.log('Triangle.js');

    initializeGrid();
    initializeValues();
    bindUIElements();     
};

var bindUIElements = function () {
    $(document).off('click', '#btnSubmit', btnSubmitClickHandler);
    $(document).on('click', '#btnSubmit', btnSubmitClickHandler);

    $(document).off('change', '#row', rowChangeHandler);
    $(document).on('change', '#row', rowChangeHandler);

    $(document).off('change', '#column', columnChangeHandler);
    $(document).on('change', '#column', columnChangeHandler);
};

var btnSubmitClickHandler = function () {
    var v1x = $('#v1x').val();
    var v1y = $('#v1y').val();
    var v2x = $('#v2x').val();
    var v2y = $('#v2y').val();
    var v3x = $('#v3x').val();
    var v3y = $('#v3y').val();

    $.ajax({
        type: "POST",
        url: 'api/triangle',
        data:
            JSON.stringify({
                "V1x": v1x,
                "V1y": v1y,
                "V2x": v2x,
                "V2y": v2y,
                "V3x": v3x,
                "V3y": v3y
            }),
        success: function (data) {
            //Update the dropdowns
            updateDropDowns(data);
        },
        dataType: 'json',
        contentType: 'application/json; charset=utf-8'
    });
};

var rowChangeHandler = function () {
    var row = $('#row').val();
    var col = $('#column').val();
    $.ajax({
        type: "GET",
        url: 'api/triangle?t=' + row + col,
        success: function (data) {
            updateCoordinateInputs(data);
        },
        dataType: 'json',
        contentType: 'application/json; charset=utf-8'
    });
};

var columnChangeHandler = function () {
    var row = $('#row').val();
    var col = $('#column').val();
    $.ajax({
        type: "GET",
        url: 'api/triangle?t=' + row + col,
        success: function (data) {
            //Update the inputs
            updateCoordinateInputs(data);
        },
        dataType: 'json',
        contentType: 'application/json; charset=utf-8'
    });
};

var initializeGrid = function () {
    // Build the grid like in the example provided to me
    var canvas = document.getElementById('myCanvas');
    if (canvas.getContext) {
        var context = canvas.getContext('2d');
        context.clearRect(0, 0, 300, 300);
        context.beginPath();

        for (var x = 50; x < 301; x += 50) {
            context.moveTo(x, 0);
            context.lineTo(x, 301);
        }

        for (var y = 50; y < 301; y += 50) {
            context.moveTo(0, y);
            context.lineTo(300, y);
        }

        var j = 300;
        for (var i = 0; i < 301; i += 50) {            
            context.moveTo(i, 0);
            context.lineTo(300, j);

            context.moveTo(0, i);
            context.lineTo(j, 300);
            j = j - 50;
        }

        context.strokeStyle = "#00F";
        context.stroke();
    }
};

var initializeValues = function () {
    $('#v1x').val(0);
    $('#v1y').val(10);
    $('#v2x').val(0);
    $('#v2y').val(0);
    $('#v3x').val(10);
    $('#v3y').val(10);
    drawTriangle(0, 10, 0, 0, 10, 10);
};

var drawTriangle = function (v1x, v1y, v2x, v2y, v3x, v3y) {
    // Using an HTML5 canvas for the first time, it's pretty cool
    initializeGrid();

    var canvas = document.getElementById('myCanvas');
    if (canvas.getContext) {
        var context = canvas.getContext('2d');
        context.beginPath();

        context.fillStyle = "#F9A520";
        context.moveTo(v1x * 5, v1y * 5);
        context.lineTo(v2x * 5, v2y * 5);
        context.lineTo(v3x * 5, v3y * 5);
        context.fill();
    }
};

var updateCoordinateInputs = function (data) {
    $('#v1x').val(data.V1x);
    $('#v1y').val(data.V1y);
    $('#v2x').val(data.V2x);
    $('#v2y').val(data.V2y);
    $('#v3x').val(data.V3x);
    $('#v3y').val(data.V3y);

    drawTriangle(data.V1x, data.V1y, data.V2x, data.V2y, data.V3x, data.V3y);
};

var updateDropDowns = function (data) {
    if (!data.IsSuccessful) {
        alert('An error occured while trying to convert the coordinates into row and column. The values passed in might not correspond to a right triangle.')
    }
    else {
        console.log(data);
        $('#row').val(data.Row);
        $('#column').val(data.Column);

        var v1x = $('#v1x').val();
        var v1y = $('#v1y').val();
        var v2x = $('#v2x').val();
        var v2y = $('#v2y').val();
        var v3x = $('#v3x').val();
        var v3y = $('#v3y').val();

        drawTriangle(v1x, v1y, v2x, v2y, v3x, v3y);
    }
};

