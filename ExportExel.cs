using OfficeOpenXml.Style;
using OfficeOpenXml;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace TuntiPorttiUser
{
    internal class ExportExel
    {
        public static void ExportToExcel(List<WorkEntry> workEntries, TimeSpan totalFlex, string userID)
        {
            try
            {
                // Set the license context for EPPlus (required since version 5.0)
                ExcelPackage.LicenseContext = LicenseContext.NonCommercial;

                using (var package = new ExcelPackage())
                {
                    // Create a worksheet
                    var worksheet = package.Workbook.Worksheets.Add("Work Entries");

                    // Add headers
                    worksheet.Cells[1, 1].Value = "Date";
                    worksheet.Cells[1, 2].Value = "Start Time";
                    worksheet.Cells[1, 3].Value = "End Time";
                    worksheet.Cells[1, 4].Value = "Total Hours";
                    worksheet.Cells[1, 5].Value = "Project Name";


                    // Format header row
                    using (var headerRange = worksheet.Cells[1, 1, 1, 6])
                    {
                        headerRange.Style.Font.Bold = true;
                        headerRange.Style.Fill.PatternType = ExcelFillStyle.Solid;
                        headerRange.Style.Fill.BackgroundColor.SetColor(System.Drawing.Color.LightGray);
                    }

                    // Add data
                    int row = 2; // Start from the second row
                    foreach (var entry in workEntries)
                    {
                        worksheet.Cells[row, 1].Value = entry.Date.ToString("yyyy-MM-dd");
                        worksheet.Cells[row, 2].Value = entry.StartTime.ToString(@"hh\:mm");
                        worksheet.Cells[row, 3].Value = entry.EndTime.ToString(@"hh\:mm");
                        worksheet.Cells[row, 4].Value = entry.TotalHours.ToString(@"hh\:mm");
                        worksheet.Cells[row, 5].Value = entry.ProjectName;
  

                        row++;
                    }
                    // Add total flex at the end
                    worksheet.Cells[row + 1, 4].Value = "Total Flex:";
                    worksheet.Cells[row + 1, 5].Value = totalFlex.ToString(); // Assuming lblTotalFlex contains the total flex time in "hh:mm" format

                    // Format total flex row
                    worksheet.Cells[row + 1, 4, row + 1, 5].Style.Font.Bold = true;

                    // Auto-fit columns
                    worksheet.Cells[worksheet.Dimension.Address].AutoFitColumns();

                    // Save the file
                    using (var saveFileDialog = new SaveFileDialog())
                    {
                        saveFileDialog.Filter = "Excel Files|*.xlsx";
                        saveFileDialog.Title = "Save Work Entries as Excel File";
                        saveFileDialog.FileName = "WorkEntries_" + userID + ".xlsx";

                        if (saveFileDialog.ShowDialog() == DialogResult.OK)
                        {
                            FileInfo fileInfo = new FileInfo(saveFileDialog.FileName);
                            package.SaveAs(fileInfo);

                            MessageBox.Show("Excel file saved successfully!", "Export Successful", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("An error occurred while exporting to Excel: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}
