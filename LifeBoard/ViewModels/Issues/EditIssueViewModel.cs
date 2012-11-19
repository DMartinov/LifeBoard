﻿using System.Collections.Generic;
using System.Linq;
using System.Windows.Input;
using LifeBoard.Models;

namespace LifeBoard.ViewModels.Issues
{
    public class EditIssueViewModel : EditIssueViewModelBase
    {
        private Issue _issue;

        public EditIssueViewModel(INavigatePage parent, BoardService boardService)
            : base(parent, boardService)
        {
            SubmitTitle = "Edit";
        }

        public void SetIssue(Issue issue)
        {
            _issue = issue;
            FromIssue();
            ParentIssues.Clear();
            foreach (var parent in BoardService.GetParents(issue.Id))
            {
                ParentIssues.Add(new IssueViewModel(this, parent));
            }
        }

        protected override void Submit()
        {
            ToIssue();
            int id = _issue.Id;
            var parents = ParentIssues.Select(pi => pi.Model.Id);
            BoardService.SetParents(id, parents);
            BoardService.Submit();
            base.Submit();
        }

        private void FromIssue()
        {
            Type = _issue.Type;
            Priority = _issue.Priority;
            Summary = _issue.Summary;
            Description = _issue.Description;
        }

        private void ToIssue()
        {
            _issue.Type = Type;
            _issue.Priority = Priority;
            _issue.Summary = Summary;
            _issue.Description = Description;
        }

        protected override IEnumerable<Issue> GetFilterIssues()
        {
            return BoardService.GetIssuesExeptChildren(_issue.Id, Filter.ToModel());
        }
    }
}
