var Diagram = function (selector, points, links) {
    $(selector).append('<canvas id="canvas"></canvas>');
    var radius = 10;
    var canvas = document.getElementById('canvas');
    canvas.width = 300;
    canvas.height = 200;
    var context = canvas.getContext('2d');

    var drawLine = function (point1, point2) {
        context.beginPath();
        context.moveTo(point1[0], point1[1]);
        context.lineTo(point2[0], point2[1]);
        context.lineWidth = 1;
        context.stroke();
    }

    var drawCircle = function (point, color, rad) {
        context.beginPath();
        context.arc(point[0], point[1], rad, 0, 2 * Math.PI);
        context.fillStyle = color;
        context.fill();
        context.closePath();
    }

    var refreshLines = function () {
        for (var i = 0; i != links.length; i++) {
            drawLine(points[links[i][0]], points[links[i][1]]);
        }
    }

    var refreshCircles = function () {
        for (var i = 0; i != points.length; i++) {
            drawCircle(points[i], 'blue', radius);
        }
    }

    refreshLines();
    refreshCircles();

    drawCircle(points[0], 'cyan', radius);
    var r = radius;
    Diagram.prototype.SetMaxRadius = function () {
        r = radius;
    }
    Diagram.prototype.SetMinRadius = function () {
        r = 0;
    }
    var point = points[0];
    Diagram.prototype.point = function (pointNum) {
        point = points[pointNum];
    };
    Diagram.prototype.smallerCircle = function () {
        drawCircle(point, 'blue', radius);
        drawCircle(point, 'cyan', r);
        r--;
        if (r >= 0) {
            setTimeout(Diagram.prototype.smallerCircle, 100);
        }
    }
    Diagram.prototype.biggerCircle = function () {
        drawCircle(point, 'cyan', r);
        r++;
        if (r <= radius) {
            setTimeout(Diagram.prototype.biggerCircle, 100);
        }
    }
    var k;
    var b;
    var point1;
    var point2;
    var prevPoint;
    Diagram.prototype.lineEquation = function (point1num, point2num) {
        point = points[point1num];
        point1 = points[point1num].slice();
        point2 = points[point2num].slice();
        k = (point2[1] - point1[1]) / (point2[0] - point1[0]);
        b = point1[1] - k * point1[0];
        prevPoint = undefined;
    }
    Diagram.prototype.goingToCircle = function () {
        if (prevPoint != undefined) {
            drawCircle(prevPoint, '#f5f5f5', radius + 1);
        }

        refreshLines();
        refreshCircles();

        prevPoint = point1.slice();
        point1[0]++;
        point1[1] = Math.floor(k * point1[0] + b);
        drawCircle(point1, 'cyan', radius);
        if (point1[0] != point2[0] && point1[1] != point2[1]) {
            setTimeout(Diagram.prototype.goingToCircle, 5);
        }
        else {
            drawCircle(prevPoint, '#f5f5f5', radius);
            drawLine(point, point2);
            drawCircle(point, 'blue', radius);
            drawCircle(point2, 'cyan', radius);
        }
    }
}