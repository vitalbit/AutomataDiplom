angular.module('tableModule', [])
	.controller('tableController', ['$scope','$compile', function ($scope, $compile) {
		var col = 1;
		var row = 1;

		var compileTemplate = function(templ, parent, scope) {
			($compile(templ)(scope)).fadeIn().appendTo(parent);
		}

		$scope.valObject = {};
		$scope.valObject.Regex = '';
		$scope.valObject.Decomposition = '';
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
		    $scope.valObject.Id = $('#testArea input[type="hidden"]')[0].value;
		    $scope.valObject.DllFilePath = $('#testArea > input[type="hidden"]')[1].value;
		    $scope.valObject.ResolveDllType = $('#testArea > input[type="hidden"]')[2].value;
		    $scope.valObject.TestFileNumber = $('#testArea > input[type="hidden"]')[3].value;
		    $scope.valObject.Description = $('#modelDescription').text();
			$scope.valObject.Cols = col;
			$scope.valObject.Rows = row;
			var TestResultsModel = $scope.valObject;
			$.ajax({
			    method: "POST",
			    url: "/Test/CompareResults",
			    data: JSON.stringify({ result: JSON.stringify(TestResultsModel) }),
			    contentType: 'application/json',
			    dataType: 'html'
			})
            .done(function (res) {
                var msg = JSON.parse(res);
                $('#targetArea').html('Your mark: ' + msg.Mark + '<br/><a href="/Home/HomePage">Back to home</a>');
            });
			//console.log($scope.valObject);
		}
	}]);

(function () {
    var testId = $('#testArea > input[type="hidden"]')[0].value;
    var dllFilePath = $('#testArea > input[type="hidden"]')[1].value;
    var resolveDllType = $('#testArea > input[type="hidden"]')[2].value;
    var fileNumber = $('#testArea > input[type="hidden"]')[3].value;

    $.ajax({
        method: "POST",
        url: "/TestPassing/CurrentFile",
        data: { testId: testId, dllFile: dllFilePath, resolveType: resolveDllType, testFileNumber: fileNumber }
    })
    .done(function (msg) {
        var targetArea = $('#targetArea');
        targetArea.attr('ng-controller', 'tableController');
        targetArea.append('<label>Description:</label><br/>' +
            '<span id="modelDescription" style="white-space: pre-line">' + msg.Description + '</span><br/>' +
            '<label>Regular expression: </label> ' +
            '<input type="text" placeholder="Regex" ng-model="valObject.Regex"/><br/>' +
            '<label>Alphabet decomposition</label><br/>' +
            '<textarea ng-model="valObject.Decomposition"></textarea>' +
            '<br/><br/>' +
            '<input type="button" class="add add-row" alt="Add row" title="Добавить ряд" ng-click="addRow()"/>' +
            '<input type="button" class="add add-column" alt="Add col" title="Добавить столбец" ng-click="addCol()"/>' +
            '<input type="button" class="add delete-row" alt="Del row" title="Удалить ряд" ng-click="delRow()"/>' +
            '<input type="button" class="add delete-col" alt="Del col" title="Удалить столбец" ng-click="delCol()"/>' +
            '<br/><br/>' +
            '<table class="automata-table" border="1">' +
                '<tr><td>{{ valObject.TableArray[0][0] }}</td></tr>' +
            '</table>' +
            '<br/><input type="submit" value="submit" ng-click="sendValues()"/>');

        angular.element(document).ready(function () {
            angular.bootstrap(document, ['tableModule']);
        });
    })
})();