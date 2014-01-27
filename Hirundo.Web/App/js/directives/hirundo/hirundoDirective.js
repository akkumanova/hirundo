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
          var image = $scope.image ? $scope.image.dataURL : null;
          var hirundo = {
            content: $scope.model,
            location: $scope.location,
            image: image
          };

          $scope.send({ hirundo: hirundo }).then(function () {
            $scope.model = null;
            $scope.location = null;
            $scope.image = null;
            $scope.focused(false);
          });
        };

        $scope.findLocation = function () {
          if ($scope.location) {
            $scope.location = null;
            $scope.$digest();
            return;
          }

          navigator.geolocation.getCurrentPosition(function (position) {
            var address = 'http://maps.googleapis.com/maps/api/geocode/json?latlng=' +
                position.coords.latitude + ',' +
                position.coords.longitude + '&sensor=true';

            $.ajax({
              type: 'GET',
              url: address,
              success: function (data) {
                $scope.location = data.results[0].address_components[2].long_name + ', ' +
                  data.results[0].address_components[4].short_name;
                $scope.$digest();
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