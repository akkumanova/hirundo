// Usage: <hd-hirundo send="<fn>" rows="<num>" placeholder="<text>">
// </hd-hirundo>

/*global angular, $, navigator*/
(function (angular, $) {
  'use strict';

  function HirundoDirective() {
    return {
      restrict: 'E',
      replace: true,
      templateUrl: 'directives/hirundo/hirundoDirective.html',
      scope: {
        placeholder: '@',
        send: '&'
      },
      link: function ($scope, element, attr) {
        var textarea = element.find('textarea');

        $scope.fixed = attr.fixed !== undefined;
        $scope.model = null;

        $scope.focused = function (isFocused) {
          if (isFocused === true || $scope.model) {
            $scope.collapsed = false;
            textarea.prop('rows', attr.rows);
          }
          else {
            $scope.collapsed = true;
            textarea.prop('rows', 1);
          }
        };
        $scope.focused($scope.fixed);

        $scope.sendHirundo = function () {
          $scope.send({ hirundo: $scope.model }).then(function () {
            $scope.model = null;
            $scope.focused(false);
          });
        };

        $scope.findLocation = function () {
            navigator.geolocation.getCurrentPosition(function (position) {
                var address = 'http://maps.googleapis.com/maps/api/geocode/json?latlng=' +
                  position.coords.latitude + ',' +
                  position.coords.longitude + '&sensor=true';

                $.ajax({
                    type: 'GET',
                    url: address,
                    success: function (data) {
                        var newData = data.results[0].formatted_address;
                        textarea.val(textarea.val() + " Posted -at " + newData);
                    },
                    dataType: 'json'
                });
            });
        };
      }
    };
  }
  angular.module('directives').directive('hdHirundo', HirundoDirective);
}(angular, $));