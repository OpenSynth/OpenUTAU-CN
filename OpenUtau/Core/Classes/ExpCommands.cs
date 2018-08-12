﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using OpenUtau.Core.USTx;

namespace OpenUtau.Core
{
    public abstract class ExpCommand : UCommand
    {
        public UVoicePart Part;
        public UNote Note;
        public string Key;
    }

    public class SetIntExpCommand : ExpCommand
    {
        public int NewValue, OldValue;
        public SetIntExpCommand(UVoicePart part, UNote note, string key, int newValue)
        {
            this.Part = part;
            this.Note = note;
            this.Key = key;
            this.NewValue = newValue;
            this.OldValue = (int)Note.Expressions[Key].Data;
        }
        public override string ToString() { return "Set note expression " + Key; }
        public override void Execute() { Note.Expressions[Key].Data = NewValue; }
        public override void Unexecute() { Note.Expressions[Key].Data = OldValue; }
    }

    public abstract class PitchExpCommand : ExpCommand { }

    public class DeletePitchPointCommand : PitchExpCommand
    {
        public int Index;
        public PitchPoint Point;
        public DeletePitchPointCommand(UVoicePart part, UNote note, int index)
        {
            this.Part = part;
            this.Note = note;
            this.Index = index;
            this.Point = Note.PitchBend.Points[Index];
        }
        public override string ToString() { return "Delete pitch point"; }
        public override void Execute() { Note.PitchBend.Points.RemoveAt(Index); }
        public override void Unexecute() { Note.PitchBend.Points.Insert(Index, Point); }
    }

    public class ChangePitchPointShapeCommand : PitchExpCommand
    {
        public PitchPoint Point;
        public PitchPointShape NewShape;
        public PitchPointShape OldShape;
        public ChangePitchPointShapeCommand(PitchPoint point, PitchPointShape shape)
        {
            this.Point = point;
            this.NewShape = shape;
            this.OldShape = point.Shape;
        }
        public override string ToString() { return "Change pitch point shape"; }
        public override void Execute() { Point.Shape = NewShape; }
        public override void Unexecute() { Point.Shape = OldShape; }
    }

    public class SnapPitchPointCommand : PitchExpCommand
    {
        double X;
        double Y;
        public SnapPitchPointCommand(UNote note)
        {
            this.Note = note;
            this.X = Note.PitchBend.Points.First().X;
            this.Y = Note.PitchBend.Points.First().Y;
        }
        public override string ToString() { return "Toggle pitch snap"; }
        public override void Execute()
        {
            Note.PitchBend.SnapFirst = !Note.PitchBend.SnapFirst;
            if (!Note.PitchBend.SnapFirst)
            {
                Note.PitchBend.Points.First().X = this.X;
                Note.PitchBend.Points.First().Y = this.Y;
            }
        }
        public override void Unexecute()
        {
            Note.PitchBend.SnapFirst = !Note.PitchBend.SnapFirst;
            if (!Note.PitchBend.SnapFirst)
            {
                Note.PitchBend.Points.First().X = this.X;
                Note.PitchBend.Points.First().Y = this.Y;
            }
        }
    }

    public class AddPitchPointCommand : PitchExpCommand
    {
        public int Index;
        public PitchPoint Point;
        public AddPitchPointCommand(UNote note, PitchPoint point, int index)
        {
            this.Note = note;
            this.Index = index;
            this.Point = point;
        }
        public override string ToString() { return "Add pitch point"; }
        public override void Execute() { Note.PitchBend.Points.Insert(Index, Point); }
        public override void Unexecute() { Note.PitchBend.Points.RemoveAt(Index); }
    }

    public class MovePitchPointCommand : PitchExpCommand
    {
        public PitchPoint Point;
        public double DeltaX, DeltaY;
        public MovePitchPointCommand(PitchPoint point, double deltaX, double deltaY)
        {
            this.Point = point;
            this.DeltaX = deltaX;
            this.DeltaY = deltaY;
        }
        public override string ToString() { return "Move pitch point"; }
        public override void Execute() { Point.X += DeltaX; Point.Y += DeltaY; }
        public override void Unexecute() { Point.X -= DeltaX; Point.Y -= DeltaY; }
    }
}
