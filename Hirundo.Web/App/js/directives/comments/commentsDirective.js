// Usage: <hd-comments category="<title>" resource="<user_resource>" user-id="<id>"></hd-comments>

/*global angular*/
(function (angular) {
  'use strict';

  function CommentsDirective($navigation, $window, User) {
    function CommentsLink($scope, element, attrs) {
      var take = $window.config.itemsToTake,
          skip = take,
          userId = attrs.user,
          resource = attrs.resource,
          moreComments = true;

      $navigation.loading = true;

      User[resource].query({ userId: userId, take: take }).$promise.then(function (comments) {
        $scope.comments = comments;
        $navigation.loading = false;
      });

      $scope.loadMore = function () {
        if (moreComments && !$scope.pending && !$navigation.loading) {
          var promise = User[resource].query({ userId: userId, take: take, skip: skip }).$promise;

          $scope.pending = true;
          promise.then(function (comments) {
            $scope.comments = $scope.comments.concat(comments);
            skip += take;
            $scope.pending = false;

            moreComments = comments.length !== 0;
          });

          return promise;
        }
      };
    }

    return {
      restrict: 'E',
      replace: true,
      templateUrl: 'directives/comments/commentsDirective.html',
      scope: {
        category: '@'
      },
      link: CommentsLink
    };
  }

  CommentsDirective.$inject = ['navigation.NavigationConfig', '$window', 'User'];

  angular.module('directives').directive('hdComments', CommentsDirective);
}(angular));