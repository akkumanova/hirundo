/*global angular*/
(function (angular) {
  'use strict';

  function WhenScrolledDirective($window) {
    return {
      restrict: 'A',
      link: function ($scope, element, attr) {
        var raw = element[0],
            haveMore = true;

        angular.element($window).bind('scroll', function () {
          if (haveMore &&
              $window.innerHeight + $window.pageYOffset >= raw.scrollHeight + raw.offsetTop) {
            $scope.$eval(attr.hdWhenScrolled).then(function (result) {
              haveMore = result;
            });
          }
        });
      }
    };
  }

  WhenScrolledDirective.$inject = ['$window'];

  angular.module('directives').directive('hdWhenScrolled', WhenScrolledDirective);
}(angular));