import { Injectable } from '@angular/core';
import { Workbook } from 'exceljs';
import * as fs from 'file-saver';
import * as Excel from 'exceljs';

@Injectable({
  providedIn: 'root',
})
export class ExcelServiceService {
  constructor() { }

  exportExcelForProjectWiseCountReport(ownerData: any) {
    //Title, Header & Data
    const title = ownerData.title;
    const header = [
      'Project Name',
      'NewTask',
      'Work In Progress',
      'UnAssigned',
      'Pending',
      'Overdue',
      'Closed',
      'Invalid',
      'Delegated',
      'Completed',
    ];
    const data = ownerData.data;

    //Create a workbook with a worksheet
    let workbook = new Excel.Workbook();
    let worksheet = workbook.addWorksheet(ownerData.title);

    // Add Row and formatting
    worksheet.mergeCells('A1', 'H3');
    let titleRow = worksheet.getCell('A1');
    titleRow.value = title;
    titleRow.font = {
      name: 'Calibri',
      size: 16,
      underline: 'single',
      bold: true,
      color: { argb: '0085A3' },
    };
    titleRow.alignment = { vertical: 'middle', horizontal: 'center' };

    // Date
    worksheet.mergeCells('I1', 'J3');
    let d = new Date();
    let date = d;
    let dateCell = worksheet.getCell('I1');

    dateCell.value = date;
    dateCell.font = {
      name: 'Calibri',
      size: 12,
      bold: true,
    };
    dateCell.alignment = { vertical: 'middle', horizontal: 'center' };

    //Adding Header Row
    let headerRow = worksheet.addRow(header);
    headerRow.eachCell((cell, number) => {
      cell.fill = {
        type: 'pattern',
        pattern: 'solid',
        fgColor: { argb: '4167B8' },
        bgColor: { argb: '' },
      };
      cell.font = {
        bold: true,
        color: { argb: 'FFFFFF' },
        size: 12,
      };
      titleRow.alignment = { vertical: 'middle', horizontal: 'center' };
    });

    // Adding Data with Conditional Formatting
    data.forEach((d: any) => {
      let row = worksheet.addRow(d);
    });

    worksheet.getColumn(1).width = 50;
    worksheet.getColumn(2).width = 10;
    worksheet.getColumn(3).width = 10;
    worksheet.getColumn(4).width = 10;
    worksheet.getColumn(5).width = 10;
    worksheet.getColumn(6).width = 10;
    worksheet.getColumn(7).width = 10;
    worksheet.getColumn(8).width = 10;
    worksheet.getColumn(9).width = 10;
    worksheet.getColumn(10).width = 10;
    worksheet.addRow([]);

    //Generate & Save Excel File
    workbook.xlsx.writeBuffer().then((data) => {
      let blob = new Blob([data], {
        type: 'application/vnd.openxmlformats-officedocument.spreadsheetml.sheet',
      });
      fs.saveAs(blob, title + '.xlsx');
    });
  }

  exportExcelFoUserWiseCountReport(ownerData: any) {
    //Title, Header & Data
    const title = ownerData.title;
    const header = [
      'First Name',
      'Last Name',
      'Department Name',
      'NewTask',
      'Work In Progress',
      'UnAssigned',
      'Pending',
      'Overdue',
      'Closed',
      'Invalid',
      'Delegated',
      'Completed',
    ];
    const data = ownerData.data;

    //Create a workbook with a worksheet
    let workbook = new Excel.Workbook();
    let worksheet = workbook.addWorksheet(title);

    // Add Row and formatting
    worksheet.mergeCells('A1', 'J3');
    let titleRow = worksheet.getCell('A1');
    titleRow.value = title;
    titleRow.font = {
      name: 'Calibri',
      size: 16,
      underline: 'single',
      bold: true,
      color: { argb: '0085A3' },
    };
    titleRow.alignment = { vertical: 'middle', horizontal: 'center' };

    // Date
    worksheet.mergeCells('K1', 'L3');
    let d = new Date();
    let date = d;
    let dateCell = worksheet.getCell('K1');

    dateCell.value = date;
    dateCell.font = {
      name: 'Calibri',
      size: 12,
      bold: true,
    };
    dateCell.alignment = { vertical: 'middle', horizontal: 'center' };

    //Adding Header Row
    let headerRow = worksheet.addRow(header);
    headerRow.eachCell((cell, number) => {
      cell.fill = {
        type: 'pattern',
        pattern: 'solid',
        fgColor: { argb: '4167B8' },
        bgColor: { argb: '' },
      };
      cell.font = {
        bold: true,
        color: { argb: 'FFFFFF' },
        size: 12,
      };
      titleRow.alignment = { vertical: 'middle', horizontal: 'center' };
    });

    // Adding Data with Conditional Formatting
    data.forEach((d: any) => {
      let row = worksheet.addRow(d);
    });

    worksheet.getColumn(1).width = 25;
    worksheet.getColumn(2).width = 25;
    worksheet.getColumn(3).width = 30;
    worksheet.getColumn(4).width = 10;
    worksheet.getColumn(5).width = 10;
    worksheet.getColumn(6).width = 10;
    worksheet.getColumn(7).width = 10;
    worksheet.getColumn(8).width = 10;
    worksheet.getColumn(9).width = 10;
    worksheet.getColumn(10).width = 10;
    worksheet.getColumn(11).width = 10;
    worksheet.getColumn(12).width = 10;
    worksheet.addRow([]);

    //Generate & Save Excel File
    workbook.xlsx.writeBuffer().then((data) => {
      let blob = new Blob([data], {
        type: 'application/vnd.openxmlformats-officedocument.spreadsheetml.sheet',
      });
      fs.saveAs(blob, title + '.xlsx');
    });
  }

  exportExcelFoUserWiseReport(ownerData: any) {
    
    //Title, Header & Data
    const title = ownerData.title;
    const header = [
      'Task Title',
      'Project Name',
      'Action Type',
      'Department',
      'Description',
      'Assigned To',
      'Assigned By',
      'Assigned Date',
      'Expected Start Date',
      'Expected End Date',
      'Actual Start Date',
      'Actual End Date',
      'Status'
    ];
    const data = ownerData.data;

    //Create a workbook with a worksheet
    let workbook = new Excel.Workbook();
    let worksheet = workbook.addWorksheet(ownerData.title);

    // Add Row and formatting
    worksheet.mergeCells('A1', 'K3');
    let titleRow = worksheet.getCell('A1');
    titleRow.value = title;
    titleRow.font = {
      name: 'Calibri',
      size: 16,
      underline: 'single',
      bold: true,
      color: { argb: '0085A3' },
    };
    titleRow.alignment = { vertical: 'middle', horizontal: 'center' };

    // Date
    worksheet.mergeCells('L1', 'M3');
    let d = new Date();
    let date = d;
    let dateCell = worksheet.getCell('L1');

    dateCell.value = date;
    dateCell.font = {
      name: 'Calibri',
      size: 12,
      bold: true,
    };
    dateCell.alignment = { vertical: 'middle', horizontal: 'center' };

    //Adding Header Row
    let headerRow = worksheet.addRow(header);
    headerRow.eachCell((cell, number) => {
      cell.fill = {
        type: 'pattern',
        pattern: 'solid',
        fgColor: { argb: '4167B8' },
        bgColor: { argb: '' },
      };
      cell.font = {
        bold: true,
        color: { argb: 'FFFFFF' },
        size: 12,
      };
      titleRow.alignment = { vertical: 'middle', horizontal: 'center' };
    });

    // Adding Data with Conditional Formatting
    data.forEach((d: any) => {
      
      var assignedDate,startDt,endDt,actualStartDt,actualEndDt = null;
      if (d[7] != null){
        assignedDate = this.dateModify(new Date(d[7]));
      } 
      if (d[8] != null){
        startDt = this.dateModify(new Date(d[8]));
      } 
      if (d[9] != null){
        endDt = this.dateModify(new Date(d[9]));
      } 
      if (d[10] != null){
        actualStartDt = this.dateModify(new Date(d[10]));
      } 
      if (d[11] != null){
        actualEndDt = this.dateModify(new Date(d[11]));
      }
      let formattedRowData = [d[0], d[1], d[2], d[3], d[4], d[5], d[6], assignedDate, startDt, endDt, actualStartDt, actualEndDt, d[12]];
      let row = worksheet.addRow(formattedRowData);
    });

    worksheet.getColumn(1).width = 25;
    worksheet.getColumn(2).width = 25;
    worksheet.getColumn(3).width = 25;
    worksheet.getColumn(4).width = 25;
    worksheet.getColumn(5).width = 50;
    worksheet.getColumn(6).width = 20;
    worksheet.getColumn(7).width = 20;
    worksheet.getColumn(8).width = 30;
    worksheet.getColumn(9).width = 30;
    worksheet.getColumn(10).width = 30;
    worksheet.getColumn(11).width = 30;
    worksheet.getColumn(12).width = 30;
    worksheet.getColumn(13).width = 15;
    worksheet.addRow([]);

    //Generate & Save Excel File
    workbook.xlsx.writeBuffer().then((data) => {
      let blob = new Blob([data], {
        type: 'application/vnd.openxmlformats-officedocument.spreadsheetml.sheet',
      });
      fs.saveAs(blob, title + '.xlsx');
    });
  }

  dateModify(data: any): string {
    var date = ("0" + data.getDate()).slice(-2) + "-" + ("0" + (data.getMonth() + 1)).slice(-2) + "-" +
      data.getFullYear() + " " + ("0" + data.getHours()).slice(-2) + ":" + ("0" + data.getMinutes()).slice(-2);
    return date;
  }

}
