// Usage: <hd-comments category="<title>" load-more="<fn>" userImg="<img>"></hd-comments>

/*global angular*/
(function (angular) {
  'use strict';

  function CommentsDirective($window, User) {
    function CommentsLink($scope) {
      var skip = 20,
          userId = $window.user.userId;

      $scope.pending = false;

      $scope.loadMore = function () {
        if (!$scope.pending) {
          $scope.pending = true;
          return User.userComments.query({ userId: userId, skip: skip })
              .$promise.then(function (comments) {
            $scope.model = $scope.model.concat(comments);
            skip += 20;
            $scope.pending = false;

            return comments.length !== 0;
          });
        }
      };
    }

    return {
      restrict: 'E',
      replace: true,
      templateUrl: 'directives/comments/commentsDirective.html',
      scope: {
        category: '@',
        loadMore: '&',
        userImg: '=',
        model: '='
      },
      link: CommentsLink
    };
  }

  CommentsDirective.$inject = ['$window', 'User'];

  angular.module('directives').directive('hdComments', CommentsDirective);
}(angular));