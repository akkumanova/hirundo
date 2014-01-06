/*global angular*/
(function (angular) {
  'use strict';

  function WhenScrolledDirective($window) {
    return {
      restrict: 'A',
      link: function ($scope, element, attrs) {
        var raw = element[0];

        angular.element($window).bind('scroll', function () {
          if ($window.innerHeight + $window.pageYOffset >= raw.scrollHeight + raw.offsetTop) {
            $scope.$eval(attrs.hdWhenScrolled);
          }
        });
      }
    };
  }

  WhenScrolledDirective.$inject = ['$window'];

  angular.module('directives').directive('hdWhenScrolled', WhenScrolledDirective);
}(angular));