angular.module('tableModule', [])
	.controller('tableController', ['$scope','$compile', function ($scope, $compile) {
		var col = 1;
		var row = 1;

		var compileTemplate = function(templ, parent, scope) {
			($compile(templ)(scope)).fadeIn().appendTo(parent);
		}

		$scope.valObject = {};
		$scope.valObject.TableArray = [];
		$scope.valObject.TableArray[0] = [];
		$scope.valObject.TableArray[0][0] = '';

		$scope.addRow = function() {
			var trLast = $('<tr></tr>').appendTo('table.automata-table');
			$scope.valObject.TableArray[row] = [];

			for (var i = 0; i!=col; i++) {
				var td = $('<td></td>').appendTo(trLast);
				if (i == 0) {
				    $scope.valObject.TableArray[row][i] = false;
				    compileTemplate('<input type="checkbox" ng-model="valObject.TableArray[' + row + '][' + i + ']"/>q' + (row - 1) + '<br/>', td, $scope);
				}
				else {
				    $scope.valObject.TableArray[row][i] = "";
				    compileTemplate('<input type="text" ng-model="valObject.TableArray[' + row + '][' + i + ']"/>', td, $scope);
				}
			}

			row++;
		};

		$scope.addCol = function() {
			var trList = $('table.automata-table tr');

			for (var i = 0; i!=row; i++) {
			    $scope.valObject.TableArray[i][col] = "";
			    compileTemplate('<td><input type="text" ng-model="valObject.TableArray[' + i + '][' + col + ']"/></td>', trList[i], $scope);
			}

			col++;
		}

		$scope.delRow = function() {
			if (row > 1) {
				$('table.automata-table tr:last').remove();
				row--;
			}
		}

		$scope.delCol = function() {
			if (col > 1) {
				$('table.automata-table tr').each(function(index, el) {
					$(this).children().last().remove();
				});
				col--;
			}
		}

		$scope.sendValues = function () {
		    $scope.valObject.Id = $('#testArea input[type="hidden"]').val();
			$scope.valObject.Cols = col;
			$scope.valObject.Rows = row;
			var TestResultsModel = $scope.valObject;
			$.ajax({
			    method: "POST",
			    url: "/Test/CompareResults",
			    data: JSON.stringify({ result: TestResultsModel }),
			    contentType: 'application/json',
			    dataType: 'html'
			})
            .done(function (res) {
                var msg = JSON.parse(res);
                $('#targetArea').html('Your mark: ' + msg.Mark + '<br/><a href="/Home/HomePage">На главную</a>');
            });
			//console.log($scope.valObject);
		}
	}]);

(function () {
    $.ajax({
        method: "POST",
        url: "/TestPassing/CurrentFile",
        data: { testId: $('#testArea input[type="hidden"]').val() }
    })
    .done(function (msg) {
        var targetArea = $('#targetArea');
        targetArea.attr('ng-controller', 'tableController');
        targetArea.append('<label>Регулярное выражение: ' + msg.Regex + '</label>' +
            '<br/><br/>' +
            '<input type="button" class="add add-row" alt="Add row" title="Добавить ряд" ng-click="addRow()"/>' +
            '<input type="button" class="add add-column" alt="Add col" title="Добавить столбец" ng-click="addCol()"/>' +
            '<input type="button" class="add delete-row" alt="Del row" title="Удалить ряд" ng-click="delRow()"/>' +
            '<input type="button" class="add delete-col" alt="Del col" title="Удалить столбец" ng-click="delCol()"/>' +
            '<br/><br/>' +
            '<table class="automata-table" border="1">' +
                '<tr><td>{{ valObject.TableArray[0][0] }}</td></tr>' +
            '</table>' +
            '<label>Описание:</label>' +
            '<span id="modelDescription" style="white-space: pre-line">' + msg.Description + '</span>' +
            '<br/><input type="submit" value="submit" ng-click="sendValues()"/>');

        angular.element(document).ready(function () {
            angular.bootstrap(document, ['tableModule']);
        });
    })
})();