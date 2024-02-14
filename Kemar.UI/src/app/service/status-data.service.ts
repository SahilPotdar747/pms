import { Injectable } from '@angular/core';

@Injectable({
  providedIn: 'root',
})
export class StatusDataService {
  constructor() {}

  TaskStatusForFilter(): any {
    const taskStatus = [
      { key: 'Status', status: '' },
      { key: 'New Task', status: 'New Task' },
      { key: 'WIP', status: 'WIP' },
      { key: 'Completed', status: 'Completed' },
      { key: 'Closed', status: 'Closed' },
      { key: 'Canceled', status: 'Canceled' },
      { key: 'On Hold', status: 'On Hold' },
      { key: 'Invalid', status: 'Invalid' },
      { key: 'Delegated', status: 'Delegated' },
      { key: 'Pending', status: 'Pending' },
      { key: 'Reopen', status: 'Reopen' },
    ];
    return taskStatus;
  }

  getTaskStatus(): any {
    const taskStatus = [
      //{ key: 'New Task', status: 'New Task' },
      { key: 'WIP', status: 'WIP' },
      { key: 'Completed', status: 'Completed' },
      { key: 'On Hold', status: 'On Hold' },
      { key: 'Invalid', status: 'Invalid' },
    ];
    return taskStatus;
  }

  getTaskPriority(): any {
    const taskStatus = [
      { key: 'High', value: 1 },
      { key: 'Medium', value: 2 },
      { key: 'Low', value: 3 },
    ];
    return taskStatus;
  }

  getDelegatedTaskStatus(): any {
    const delegatedStatus = [
      { key: 'Requested', value: 'Requested' },
      { key: 'Accepted', value: 'Accepted' },
      { key: 'Rejected', value: 'Rejected' },
      { key: 'Reassigned', value: 'Reassigned' },
    ];
    return delegatedStatus;
  }

  getProjectStatus(): any {
    const projectStatus = [
      { key: 'All', value: '' },
      { key: 'Upcoming', value: 'Upcoming' },
      { key: 'Ongoing', value: 'Ongoing' },
      { key: 'Completed', value: 'Completed' },
      { key: 'Cancelled', value: 'Cancelled' },
      { key: 'OnHold', value: 'OnHold' },
    ];
    return projectStatus;
  }

  projectStatus(): any {
    const projectStatus = [
      { key: 'Upcoming', value: 'Upcoming' },
      { key: 'Ongoing', value: 'Ongoing' },
      { key: 'Completed', value: 'Completed' },
      { key: 'Cancelled', value: 'Cancelled' },
      { key: 'OnHold', value: 'OnHold' },
    ];
    return projectStatus;
  }
}
