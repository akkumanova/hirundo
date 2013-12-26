// Usage: <hd-hirundo model="<model_name>"></hd-hirundo>

/*global angular*/
(function (angular) {
  'use strict';

  function HirundoDirective() {
    return {
      restrict: 'E',
      replace: true,
      templateUrl: 'directives/hirundo/hirundoDirective.html',
      scope: {
        model: '=',
        send: '&'
      },
      link: function ($scope, element) {
        var textarea = element.find('textarea');

        $scope.collapsed = true;

        $scope.focused = function (isFocused) {
          if (isFocused === true || $scope.model) {
            $scope.collapsed = false;
            textarea.prop('rows', 8);
          }
          else {
            $scope.collapsed = true;
            textarea.prop('rows', 1);
          }
        };

        $scope.sendHirundo = function () {
          $scope.send().then(function () {
            $scope.model = null;
            $scope.focused(false);
          });
        };
      }
    };
  }

  angular.module('directives').directive('hdHirundo', HirundoDirective);
}(angular));