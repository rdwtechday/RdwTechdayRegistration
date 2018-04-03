using iTextSharp.text;
using iTextSharp.text.pdf;
using RdwTechdayRegistration.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace RdwTechdayRegistration.Utility
{
    public class BadgeGenerator
    {
        private const int PageHeight = 842;
        private const int PageWidth = 595;
        private const int CardWidth = 253;
        private const int CardHeight = 153;
        private const int XMargin = (PageWidth - 2 * CardWidth) / 2;
        private const int YMargin = (PageHeight - 5 * CardHeight) / 2;
        private const float BorderWidth = 0.2f;
        private const int SquareSize = 20;

        private readonly Document _document;
        private readonly PdfContentByte _contentByte;
        private readonly BaseFont _baseFont;
        private readonly Font _fontOrganization;
        private readonly Font _fontBannerOrange;
        private readonly Font _fontBannerGray;
        private readonly Image _referenceImage;
        private readonly Chunk _orangeChunk;
        private readonly Chunk _grayChunk;
        private readonly Font _fontOrganizerName;
        private readonly Font _fontSpeakerName;
        private readonly Font _fontUserName;
        private readonly Font _fontSession;

        public BadgeGenerator(Stream stream, string imagePath)
        {
            _baseFont = BaseFont.CreateFont(BaseFont.HELVETICA, BaseFont.CP1252, true);

            _fontOrganizerName = new Font(_baseFont, 24.0f, Font.BOLD, new BaseColor(112, 48, 160));
            _fontSpeakerName = new Font(_baseFont, 24.0f, Font.BOLD, new BaseColor(0, 176, 240));
            _fontUserName = new Font(_baseFont, 24.0f, Font.BOLD, new BaseColor(236, 82, 0));
            _fontSession = new Font(_baseFont, 16.0f, Font.NORMAL);
            _fontOrganization = new Font(_baseFont, 16.0f, Font.ITALIC);
            _fontBannerOrange = new Font(_baseFont, 32.0f, Font.NORMAL, new BaseColor(236, 82, 0));
            _fontBannerGray = new Font(_baseFont, 32.0f, Font.NORMAL, new BaseColor(220, 220, 220));
            _referenceImage = Image.GetInstance(imagePath);
            _referenceImage.ScaleAbsolute(CardWidth, 47.0f);

            _document = new Document(PageSize.A4);
            PdfWriter writer = PdfWriter.GetInstance(_document, stream);
            _document.Open();
            _contentByte = writer.DirectContent;
            _orangeChunk = new Chunk("RDW", _fontBannerOrange);
            _grayChunk = new Chunk(" Techday", _fontBannerGray);
        }

        private void AddPageMarkers()
        {
            _contentByte.SetLineWidth(BorderWidth);
            /* draw verticals */
            for (int i = 0; i < 3; i++)
            {
                _contentByte.MoveTo(XMargin + i * CardWidth, 0);
                _contentByte.LineTo(XMargin + i * CardWidth, PageHeight);
                _contentByte.Stroke();
            }

            /* draw horizontals */
            for (int i = 0; i < 6; i++)
            {
                _contentByte.MoveTo(0, YMargin + i * CardHeight);
                _contentByte.LineTo(PageWidth, YMargin + i * CardHeight);
                _contentByte.Stroke();
            }
        }

        private static BaseColor DeriveColorFromRgbString(string rgbValue)
        {
            if (rgbValue == null || (rgbValue.Length != 3 && rgbValue.Length != 6))
            {
                return null;
            }

            int[] raw = new int[3];
            if (rgbValue.Length == 3)
            {
                for (int i = 0; i < 3; i++)
                {
                    int hex = Convert.ToByte(rgbValue.Substring(i, 1), 16);
                    raw[i] = hex * 16 + hex;
                }
            }
            else
            {
                for (int i = 0; i < 3; i++)
                {
                    raw[i] = Convert.ToByte(rgbValue.Substring(2 * i, 2), 16);
                }
            }
            return new BaseColor(raw[0], raw[1], raw[2]);
        }

        private Font SelecctFontForPerson(BadgePersonType personType)
        {
            Font fontName = _fontUserName;

            if (personType == BadgePersonType.organizer)
            {
                fontName = _fontOrganizerName;
            }
            if (personType == BadgePersonType.speaker)
            {
                fontName = _fontSpeakerName;
            }
            return fontName;
        }

        private void AddOrganizatonCell(PdfPTable table, string organisation)
        {
            PdfPCell cell;
            if (organisation == null)
            {
                cell = new PdfPCell() { Colspan = 6 };
            }
            else
            {
                cell = new PdfPCell(new Phrase(String.Format(organisation), _fontOrganization)) { Colspan = 6 };
            }
            cell.FixedHeight = 27.0f;
            cell.BorderWidth = 0;
            cell.PaddingBottom = 10.0f;
            cell.HorizontalAlignment = Element.ALIGN_CENTER;
            cell.VerticalAlignment = Element.ALIGN_TOP;
            table.AddCell(cell);
        }

        private void AddPersonCell(PdfPTable table, BadgePersonType personType, string name)
        {
            PdfPCell cell;
            Font fontName = SelecctFontForPerson(personType);

            if (name == null)
            {
                cell = new PdfPCell() { Colspan = 6 };
            }
            else
            {
                cell = new PdfPCell(new Phrase(String.Format(name), fontName)) { Colspan = 6 };
            }
            cell.FixedHeight = 51.0f;
            cell.BorderWidth = 0;
            cell.HorizontalAlignment = Element.ALIGN_CENTER;
            cell.VerticalAlignment = Element.ALIGN_MIDDLE;
            table.AddCell(cell);
        }

        private void AddPhotoCell(PdfPTable table)
        {
            /* the cell with the photo and event name */
            Chunk orangeChunk = new Chunk(_orangeChunk);
            Chunk grayChunk = new Chunk(_grayChunk);
            var confphrase = new Phrase(orangeChunk);
            confphrase.Add(grayChunk);

            PdfPCell cell = new PdfPCell(confphrase) { Colspan = 6 };
            cell.FixedHeight = 47.0f;
            cell.BorderWidth = 0;
            cell.PaddingBottom = 10.0f;
            cell.HorizontalAlignment = Element.ALIGN_CENTER;
            cell.VerticalAlignment = Element.ALIGN_MIDDLE;
            table.AddCell(cell);
        }

        private void AddSessionCellSuffix(PdfPTable table, PdfPCell cell)
        {
            cell.FixedHeight = 21.0f;
            cell.BorderWidth = 1.1f;
            cell.BorderColor = new BaseColor(255, 255, 255);
            cell.HorizontalAlignment = Element.ALIGN_CENTER;
            cell.VerticalAlignment = Element.ALIGN_MIDDLE;
            cell.Padding = 5.0f;
        }

        /// <summary>
        /// Add empty cell
        /// </summary>
        private void AddSessionCell(PdfPTable table)
        {
            PdfPCell cell = new PdfPCell();
            cell.BackgroundColor = new BaseColor(255, 255, 255);
            AddSessionCellSuffix(table, cell);
            table.AddCell(cell);
        }

        private void AddSessionCell(PdfPTable table, ApplicationUserTijdvak applicationUserTijdvak)
        {
            PdfPCell cell;

            if (applicationUserTijdvak.Sessie == null || applicationUserTijdvak.Sessie.Track.BadgeCode == null)
            {
                cell = new PdfPCell(new Phrase(String.Format("")));
                cell.BackgroundColor = new BaseColor(255, 255, 255);
            }
            else
            {
                cell = new PdfPCell(new Phrase(String.Format(applicationUserTijdvak.Sessie.Track.BadgeCode), _fontSession));
                cell.BackgroundColor = DeriveColorFromRgbString(applicationUserTijdvak.Sessie.Track.Rgb);
            }
            AddSessionCellSuffix(table, cell);
            table.AddCell(cell);
        }

        private void AddSessionsCells(PdfPTable table, ICollection<ApplicationUserTijdvak> ApplicationUserTijdvakken)
        {
            foreach (ApplicationUserTijdvak atv in ApplicationUserTijdvakken.OrderBy(t => t.Tijdvak.Order))
            {
                AddSessionCell(table, atv);
            }
        }

        private void AddEmptySessionsCells(PdfPTable table)
        {
            for (int i = 0; i < 6; i++)
            {
                AddSessionCell(table);
            }
        }

        /// <summary>
        /// Add user from ApplicationUser model
        /// It will determine PersonType user or admin from the usermodel and show which sessions have been joined
        /// whene user is admin it will hide sessions and show "Techday Organisatie" as admin line
        /// </summary>
        private void AddCard(float absoluteX, float absoluteY, ApplicationUser user)
        {
            string organisationLine = user.Organisation;
            if (user.Department != null && user.Department.Trim().Length > 0)
            {
                organisationLine = organisationLine + "-" + user.Department;
            }
            if (user.isAdmin)
            {
                organisationLine = "Techday Organisatie";
            }
            BadgePersonType personType = user.isAdmin ? BadgePersonType.organizer : BadgePersonType.user;

            /* create a table to put the info in */
            PdfPTable table = new PdfPTable(6);
            table.SetTotalWidth(new float[] { CardWidth / 6, CardWidth / 6, CardWidth / 6, CardWidth / 6, CardWidth / 6, CardWidth / 6 });
            table.LockedWidth = true;

            AddPersonCell(table, personType, user.Name);
            AddOrganizatonCell(table, organisationLine);
            AddPhotoCell(table);
            if (user.isAdmin)
            {
                AddEmptySessionsCells(table);
            }
            else
            {
                AddSessionsCells(table, user.ApplicationUserTijdvakken);
            }

            /* write table at specified coordinates */
            table.WriteSelectedRows(0, -1, absoluteX, absoluteY + CardHeight, _contentByte);

            /* position image in the cell for them image and event name */
            Image image = Image.GetInstance(_referenceImage);
            image.SetAbsolutePosition(absoluteX, absoluteY + 22.0f);
            _document.Add(image);
        }

        /// <summary>
        /// Add a card using name, organization and persontype provided
        /// meant to be used to add users not in the database
        /// will leave the sessions cell at the bottom of the badge empty
        /// </summary>
        private void AddCard(float absoluteX, float absoluteY, BadgeContentModel badge)
        {
            /* create a table to put the info in */
            PdfPTable table = new PdfPTable(6);
            table.SetTotalWidth(new float[] { CardWidth / 6, CardWidth / 6, CardWidth / 6, CardWidth / 6, CardWidth / 6, CardWidth / 6 });
            table.LockedWidth = true;

            AddPersonCell(table, badge.PersonType, badge.name);
            if (badge.PersonType == BadgePersonType.organizer)
            {
                AddOrganizatonCell(table, "Techday Organisatie");
            }
            else
            {
                AddOrganizatonCell(table, badge.organisation);
            }
            AddPhotoCell(table);
            AddEmptySessionsCells(table);
            /* write table at specified coordinates */
            table.WriteSelectedRows(0, -1, absoluteX, absoluteY + CardHeight, _contentByte);

            /* position image in the cell for them image and event name */
            Image image = Image.GetInstance(_referenceImage);
            image.SetAbsolutePosition(absoluteX, absoluteY + 22.0f);
            _document.Add(image);
        }

        public void FillPages(List<ApplicationUser> users)
        {
            int colCount = 0;
            int rowCount = 0;

            foreach (ApplicationUser user in users)
            {
                if (rowCount > 4)
                {
                    // end of page reached, add new page, reset counters and draw grid for cutting the paper */
                    AddPageMarkers();
                    _document.NewPage();
                    rowCount = 0;
                }

                AddCard(XMargin + colCount * CardWidth, YMargin + rowCount * CardHeight, user);

                colCount++; // alternate over columns
                if (colCount > 1)
                {
                    rowCount++; /* add a row when starting at column 0 */
                    colCount = 0;
                }
            }
            AddPageMarkers();
            _document.Close();
        }

        public void FillPages(List<BadgeContentModel> badges)
        {
            int colCount = 0;
            int rowCount = 0;

            foreach (BadgeContentModel badge in badges)
            {
                if (rowCount > 4)
                {
                    // end of page reached, add new page, reset counters and draw grid for cutting the paper */
                    AddPageMarkers();
                    _document.NewPage();
                    rowCount = 0;
                }
                AddCard(XMargin + colCount * CardWidth, YMargin + rowCount * CardHeight, badge);

                colCount++; // alternate over columns
                if (colCount > 1)
                {
                    rowCount++; /* add a row when starting at column 0 */
                    colCount = 0;
                }
            }
            AddPageMarkers();
            _document.Close();
        }

    }
}