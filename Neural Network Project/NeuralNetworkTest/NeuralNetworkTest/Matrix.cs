using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NeuralNetworkTest
{
    public class MatrixData<T> : IEnumerator<T>, IEnumerable<T>
    {
        // Description:
        // Provides methods for creating and manipulating matricies...

        // Data
        private List<List<T>> rowData;
        private List<List<T>> columnData;
        private int rows;
        private int columns;
        private T defaultData;
        private int row = -1;
        private int column = -1;

        // Accessor Methods
        public T[,] Data { get => FetchArray(); set => UpdateArray(value); }
        public int Rows { get => rows;}
        public int Columns { get => columns;}
        public List<List<T>> RowData { get => rowData; set => SetRowData(value); }
        public List<List<T>> ColumnData { get => columnData; set => SetColumnData(value); }
        public int Row { get => row; set => row = value; }
        public int Column { get => column; set => column = value; }

        // Indexers
        public T this[int row, int column] { get => Data[row,column]; set => InsertValue(row, column, value); }

        public void SetRow(int index, List<T> value)
        {
            // Replaces a row in the matrix with the one supplied
            RowData[index] = value;
            UpdateData("Row", index);
        }

        public T[] GetRow(int index)
        {
            // Replaces a row in the matrix with the one supplied
            return RowData[index].ToArray();
        }

        public void SetColumn(int index, List<T> value)
        {
            // Replaces a row in the matrix with the one supplied
            ColumnData[index] = value;
            UpdateData("Column", index);
        }

        public T[] GetColumn(int index)
        {
            // Replaces a row in the matrix with the one supplied
            return ColumnData[index].ToArray();
        }

        // Helper Functions

        private void InsertValue(int row, int column, T value)
        {
            // Inserts an element into the matrix

            // Updates the rowData
            List<T> tempRow = rowData[row];
            tempRow[column] = value;
            SetRow(row, tempRow);

            // Updates the columnData
            List<T> tempCol = columnData[column];
            tempCol[row] = value;
            SetColumn(column, tempCol);
        }

        private T[,] FetchArray()
        {
            // Generates a 2-Dimensional array out of the lists and returns it
            T[,] temp = new T[rows, columns];
            for(int i = 0; i < rows; i++)
            {
                for(int j = 0; j < columns; j++)
                {
                    temp[i, j] = rowData[i][j];
                }
            }
            return temp;
        }

        private void UpdateArray(T[,] data)
        {
            rows = data.GetLength(0);
            columns = data.GetLength(1);
            rowData = new List<List<T>>(rows);
            columnData = new List<List<T>>(columns);
            bool first = true;

            for (int i = 0; i < rows; i++)
            {
                rowData.Add(new List<T>(columns));
                for (int j = 0; j < columns; j++)
                {
                    rowData[i].Add(data[i, j]);
                    if (first)
                    {
                        columnData.Add(new List<T>(rows));
                        for (int k = 0; k < rows; k++)
                            columnData[j].Add(data[k, j]);
                    }
                }
                if (first)
                    first = false;
            }
        }

        private void SetRowData(List<List<T>> value)
        {
            rowData = value;
            UpdateData("Row");
        }

        private void SetColumnData(List<List<T>> value)
        {
            columnData = value;
            UpdateData("Column");
        }

        private void UpdateData(string update, int index)
        {
            // When the rowData or columnData or data properties are set, this function needs to be called to ensure that all of the arrays and lists get updated
            // This will allow so that when arrays are read, and passed back, if they're bigger than the original size, then the matrix will be expanded to accommodate the new data
            int diff;
            switch (update)
            {
                case ("Row"):
                    diff = RowData[index].Count - columns;
                    if (diff > 0)
                    {
                        // Creates the new columns that didn't exist yet
                        for (int i = 0; i < diff; i++)
                            ColumnData.Add(new List<T>(rows));

                        // Sets the new columns value
                        columns = RowData[index].Count;

                        // Updates all of the values in the rowData and columnData lists
                        for (int i = 0; i < rows; i++)
                        {
                            if (i != index)
                            {
                                // Pads all other rows with default data to make up the difference
                                for (int j = 0; j < diff; j++)
                                    RowData[i].Add(defaultData);
                            }
                            else
                            {
                                // Updates all of the columns incase any values have changed
                                for (int j = 0; j < columns; j++)
                                {
                                    List<T>[] temp = ColumnData.ToArray();
                                    temp[j][i] = RowData[i][j];
                                }
                            }
                        }
                    }
                    else if(diff < 0)
                    {
                        diff = columns - RowData[index].Count;

                        // pads the list with default data to make it fit.
                        for (int j = 0; j < diff; j++)
                            RowData[index].Add(defaultData);

                        // Updates all of the columns incase any values have changed
                        for (int j = 0; j < columns; j++)
                        {
                            List<T>[] temp = ColumnData.ToArray();
                            temp[j][index] = RowData[index][j];
                        }
                    }
                    else
                    {
                        // Updates all of the columns incase any values have changed
                        for (int j = 0; j < columns; j++)
                        {
                            List<T>[] temp = ColumnData.ToArray();
                            temp[j][index] = RowData[index][j];
                        }
                    }
                    break;

                case ("Column"):
                    diff = ColumnData[index].Count - rows;
                    if (diff > 0)
                    {
                        // Creates the new columns that didn't exist yet
                        for (int i = 0; i < diff; i++)
                            RowData.Add(new List<T>(columns));

                        // Sets the new columns value
                        rows = RowData[index].Count;

                        // Updates all of the values in the rowData and columnData lists
                        for (int i = 0; i < columns; i++)
                        {
                            if (i != index)
                            {
                                // Pads all other rows with default data to make up the difference
                                for (int j = 0; j < diff; j++)
                                    ColumnData[i].Add(defaultData);
                            }
                            else
                            {
                                // Updates all of the columns incase any values have changed
                                for (int j = 0; j < rows; j++)
                                {
                                    List<T>[] temp = RowData.ToArray();
                                    temp[j][i] = ColumnData[i][j];
                                }
                            }
                        }
                    }
                    else if (diff < 0)
                    {
                        diff = rows - ColumnData[index].Count;

                        // pads the list with default data to make it fit.
                        for (int j = 0; j < diff; j++)
                            ColumnData[index].Add(defaultData);

                        // Updates all of the columns incase any values have changed
                        for (int j = 0; j < rows; j++)
                        {
                            List<T>[] temp = RowData.ToArray();
                            temp[j][index] = ColumnData[index][j];
                        }
                    }
                    else
                    {
                        // Updates all of the columns incase any values have changed
                        for (int j = 0; j < rows; j++)
                        {
                            List<T>[] temp = RowData.ToArray();
                            temp[j][index] = ColumnData[index][j];
                        }
                    }
                    break;
            }
        }

        private void UpdateData(string update)
        {
            // When the rowData or columnData or data properties are set, this function needs to be called to ensure that all of the arrays and lists get updated
            // This will allow so that when arrays are read, and passed back, if they're bigger than the original size, then the matrix will be expanded to accommodate the new data
            int diff;
            switch (update)
            {
                case ("Row"):
                    diff = RowData[0].Count - columns;
                    if (diff > 0)
                    {
                        // Creates the new columns that didn't exist yet
                        for (int i = 0; i < diff; i++)
                        {
                            ColumnData.Add(new List<T>(rows));
                            for (int j = 0; j < rows; j++)
                                ColumnData[ColumnData.Count - 1][j] = defaultData;
                        }

                        // Sets the new columns value
                        columns = RowData[0].Count;

                        // Updates all of the values in the rowData and columnData lists
                        for (int i = 0; i < rows; i++)
                        {
                            // Updates all of the columns incase any values have changed
                            for (int j = 0; j < columns; j++)
                            {
                                List<T>[] temp = ColumnData.ToArray();
                                temp[j][i] = RowData[i][j];
                            }
                        }
                    }
                    else if (diff < 0)
                    {
                        diff = columns - RowData[0].Count;

                        // Updates all of the values in the rowData and columnData lists
                        for (int i = 0; i < rows; i++)
                        {
                            // Pads all other rows with default data to make up the difference
                            for (int j = 0; j < diff; j++)
                                RowData[i].Add(defaultData);

                            // Updates all of the columns incase any values have changed
                            for (int j = 0; j < columns; j++)
                            {
                                List<T>[] temp = ColumnData.ToArray();
                                temp[j][i] = RowData[i][j];
                            }
                        }
                    }
                    else
                    {
                        // Updates all of the values in the rowData and columnData lists
                        for (int i = 0; i < rows; i++)
                        {
                            // Updates all of the columns incase any values have changed
                            for (int j = 0; j < columns; j++)
                            {
                                List<T>[] temp = ColumnData.ToArray();
                                temp[j][i] = RowData[i][j];
                            }
                        }
                    }
                    break;

                case ("Column"):
                    diff = ColumnData[0].Count - rows;
                    if (diff > 0)
                    {
                        // Creates the new columns that didn't exist yet
                        for (int i = 0; i < diff; i++)
                        {
                            RowData.Add(new List<T>(columns));
                            for (int j = 0; j < columns; j++)
                                RowData[RowData.Count - 1][j] = defaultData;
                        }

                        // Sets the new columns value
                        rows = RowData[0].Count;

                        // Updates all of the values in the rowData and columnData lists
                        for (int i = 0; i < columns; i++)
                        {
                            // Updates all of the columns incase any values have changed
                            for (int j = 0; j < rows; j++)
                            {
                                List<T>[] temp = RowData.ToArray();
                                temp[j][i] = ColumnData[i][j];
                            }
                        }
                    }
                    else if (diff < 0)
                    {
                        diff = rows - ColumnData[0].Count;

                        // Updates all of the values in the rowData and columnData lists
                        for (int i = 0; i < columns; i++)
                        {
                            // Pads all other rows with default data to make up the difference
                            for (int j = 0; j < diff; j++)
                                ColumnData[i].Add(defaultData);

                            // Updates all of the columns incase any values have changed
                            for (int j = 0; j < rows; j++)
                            {
                                List<T>[] temp = RowData.ToArray();
                                temp[j][i] = ColumnData[i][j];
                            }
                        }
                    }
                    else
                    {
                        // Updates all of the values in the rowData and columnData lists
                        for (int i = 0; i < columns; i++)
                        {
                            // Updates all of the columns incase any values have changed
                            for (int j = 0; j < rows; j++)
                            {
                                List<T>[] temp = RowData.ToArray();
                                temp[j][i] = ColumnData[i][j];
                            }
                        }
                    }
                    break;

                default:
                    Console.WriteLine("Unrecognized update mdoe: " + update);
                    break;
            }
        }

        // Constructor
        public MatrixData(int rows, int columns, T initialValue)
        {
            this.rows = rows;
            this.columns = columns;
            rowData = new List<List<T>>(rows);
            columnData = new List<List<T>>(columns);
            bool first = true;

            for(int i = 0; i < rows; i++)
            {
                rowData.Add(new List<T>(columns));
                for (int j = 0; j < columns; j++)
                {
                    rowData[i].Add(initialValue);

                    if (first)
                    {
                        columnData.Add(new List<T>(rows));
                        for (int k = 0; k < rows; k++)
                            columnData[j].Add(initialValue);
                    }
                }
                if (first)
                    first = false;
            }

            //Console.WriteLine("Created a new matrix with size [{0}, {1}]", rows, columns);
        }

        public MatrixData(T initialValue)
        {
            rows = 1;
            columns = 1;
            RowData = new List<List<T>>(rows);
            ColumnData = new List<List<T>>(columns);

            for (int i = 0; i < rows; i++)
            {
                RowData[i] = new List<T>(columns);
                for (int j = 0; j < columns; j++)
                {
                    RowData[i][j] = initialValue;
                    ColumnData[j] = new List<T>(rows);
                    for (int k = 0; k < rows; k++)
                        ColumnData[j][k] = initialValue;
                }
            }
        }

        // IEnumerator and IEnumerable requirements

        public bool MoveNext()
        {
            if (++row > (rows - 1))
            {
                row = 0;
                if (++column > (columns - 1))
                    return true;
            }
            return false;
        }

        public void Reset()
        {
            row = 0;
            column = 0;
        }

        public object Current
        {
            get { return RowData[row][column]; }
        }
        T IEnumerator<T>.Current => RowData[row][column];

        public void Dispose()
        {
            rowData = null;
            columnData = null;
            rows = -1;
            columns = -1;
        }

        public IEnumerator<T> GetEnumerator()
        {
            return this;
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return this;
        }
    }

    public enum IdentityMult { BEFORE=0, AFTER=1}

    class Matrix
    {
        // Provides functionality for creating and manipulating matrix data

        // Data
        private MatrixData<double> data;
        private double defaultData = 0;

        // Accessor Methods
        private MatrixData<double> Data { get => data; set => data = value; }
        public double DefaultData { get => defaultData; set => defaultData = value; }
        public int Size { get => (data.Rows * data.Columns); }
        public int Rows { get => data.Rows; }
        public int Columns { get => data.Columns; }

        // Indexers
        public double this[int row, int column] { get => data[row, column]; set => data[row, column] = value; }

        // Constructors
        public Matrix(int rows, int columns, double defaultData = 0)
        {
            this.defaultData = defaultData;
            data = new MatrixData<double>(rows, columns, defaultData);
        }

        public Matrix(double[,] values)
        {
            data = new MatrixData<double>(values.GetLength(0), values.GetLength(1), 0)
            {
                Data = values
            };
            //PrintContents();
        }

        public Matrix()
        {
            defaultData = 0;
            data = new MatrixData<double>(defaultData);
        }

        // Numeric Operators

        public static Matrix operator +(Matrix m1, Matrix m2)
        {
            // Performs the mathematical operation of adding two matrices together
            if ((m1.Rows != m2.Rows) || (m1.Columns != m2.Columns))
                throw new InvalidMatrixOperationException("Cannot Add matrices of different dimensions!");
            Matrix temp = new Matrix(m1.Rows, m1.Columns);
            for(int i = 0; i < m1.Rows; i++)
            {
                for(int j = 0; j < m1.Columns; j++)
                {
                    temp[i, j] = m1[i, j] + m2[i, j];
                }
            }
            return temp;
        }

        public static Matrix operator +(Matrix m1, double value)
        {
            // Performs the mathematical operation of adding a constant to a matrix
            Matrix temp = new Matrix();
            for (int i = 0; i < m1.Rows; i++)
            {
                for (int j = 0; j < m1.Columns; j++)
                {
                    temp[i, j] = m1[i, j] + value;
                }
            }
            return temp;
        }

        public static Matrix operator +(double value, Matrix m1)
        {
            // Performs the mathematical operation of adding a constant to a matrix
            Matrix temp = new Matrix();
            for (int i = 0; i < m1.Rows; i++)
            {
                for (int j = 0; j < m1.Columns; j++)
                {
                    temp[i, j] = m1[i, j] + value;
                }
            }
            return temp;
        }

        public static Matrix operator -(Matrix m1, Matrix m2)
        {
            // Performs the mathematical operation of subtracting two matrices
            if ((m1.Rows != m2.Rows) || (m1.Columns != m2.Columns))
                throw new InvalidMatrixOperationException("Cannot Subtract matrices of different dimensions!");
            Matrix temp = new Matrix();
            for (int i = 0; i < m1.Rows; i++)
            {
                for (int j = 0; j < m1.Columns; j++)
                {
                    temp[i, j] = m1[i, j] - m2[i, j];
                }
            }
            return temp;
        }

        public static Matrix operator -(Matrix m1, double value)
        {
            // Performs the mathematical operation of subtracting a constant from a matrix
            Matrix temp = new Matrix();
            for (int i = 0; i < m1.Rows; i++)
            {
                for (int j = 0; j < m1.Columns; j++)
                {
                    temp[i, j] = m1[i, j] - value;
                }
            }
            return temp;
        }

        public static Matrix operator -(double value, Matrix m1)
        {
            // Performs the mathematical operation of subtracting a constant from a matrix
            Matrix temp = new Matrix();
            for (int i = 0; i < m1.Rows; i++)
            {
                for (int j = 0; j < m1.Columns; j++)
                {
                    temp[i, j] = m1[i, j] - value;
                }
            }
            return temp;
        }

        public static Matrix operator *(Matrix m1, Matrix m2)
        {
            // Performs the mathematical operation of multiplying two matrices together
            if (m1.Columns != m2.Rows)
                throw new InvalidMatrixOperationException("Cannot multiply matrices of different sizes!");

            Matrix temp = new Matrix(m1.Rows, m2.Columns);
            double tempRow = 0;

            for (int i = 0; i < m1.Rows; i++)
            {
                // Pulls out each row of the first matrix
                for (int j = 0; j < m2.Columns; j++)
                {
                    tempRow = Dot(m1.RowData()[i].ToArray(), m2.ColData()[j].ToArray());
                    temp[i, j] = tempRow;
                }
            }

            return temp;
        }

        public static Matrix operator *(Matrix m1, double value)
        {
            // Performs the mathematical operation of multiplying a constant to a matrix
            Matrix temp = new Matrix();
            for (int i = 0; i < m1.Rows; i++)
            {
                for (int j = 0; j < m1.Columns; j++)
                {
                    temp[i, j] = m1[i, j] * value;
                }
            }
            return temp;
        }

        public static Matrix operator *(double value, Matrix m1)
        {
            // Performs the mathematical operation of multiplying a constant to a matrix
            Matrix temp = new Matrix();
            for (int i = 0; i < m1.Rows; i++)
            {
                for (int j = 0; j < m1.Columns; j++)
                {
                    temp[i, j] = m1[i, j] * value;
                }
            }
            return temp;
        }

        public static Matrix operator /(Matrix m1, Matrix m2)
        {
            // Performs the mathematical operation of dividing 2 matrices
            return m1 * m2.Invert();
        }

        public static Matrix operator /(Matrix m1, double value)
        {
            // Performs the mathematical operation of dividing a matrix by a constant
            Matrix temp = new Matrix(m1.Rows, m1.Columns);
            for (int i = 0; i < m1.Rows; i++)
            {
                for (int j = 0; j < m1.Columns; j++)
                {
                    temp[i, j] = m1[i, j] / value;
                }
            }
            return temp;
        }

        public static Matrix operator /(double value, Matrix m1)
        {
            // Performs the mathematical operation of dividing a matrix by a constant
            Matrix temp = new Matrix(m1.Rows, m1.Columns);
            for (int i = 0; i < m1.Rows; i++)
            {
                for (int j = 0; j < m1.Columns; j++)
                {
                    temp[i, j] = m1[i, j] / value;
                }
            }
            return temp;
        }

        // Boolean Operators

        public static Matrix operator >(Matrix m1, double value)
        {
            // Returns an array evaluated element by element against m1, and m2
            Matrix temp = new Matrix(m1.Rows, m1.Columns);
            for (int i = 0; i < m1.Rows; i++)
            {
                for (int j = 0; j < m1.Columns; j++)
                {
                    temp[i, j] = (m1[i, j] > value) ? 1:0;
                }
            }
            return temp;
        }

        public static Matrix operator >(Matrix m1, Matrix m2)
        {
            // Returns an array evaluated element by element against m1, and m2
            if ((m1.Rows != m2.Rows) || (m1.Columns != m2.Columns))
                throw new InvalidMatrixOperationException("Cannot Compare matrices of different dimensions!");
            Matrix temp = new Matrix(m1.Rows, m1.Columns);
            for (int i = 0; i < m1.Rows; i++)
            {
                for (int j = 0; j < m1.Columns; j++)
                {
                    temp[i, j] = (m1[i, j] > m2[i, j]) ? 1 : 0;
                }
            }
            return temp;
        }

        public static Matrix operator <(Matrix m1, double value)
        {
            // Returns an array evaluated element by element against m1, and m2
            Matrix temp = new Matrix(m1.Rows, m1.Columns);
            for (int i = 0; i < m1.Rows; i++)
            {
                for (int j = 0; j < m1.Columns; j++)
                {
                    temp[i, j] = (m1[i, j] < value) ? 1 : 0;
                }
            }
            return temp;
        }

        public static Matrix operator <(Matrix m1, Matrix m2)
        {
            // Returns an array evaluated element by element against m1, and m2
            if ((m1.Rows != m2.Rows) || (m1.Columns != m2.Columns))
                throw new InvalidMatrixOperationException("Cannot Compare matrices of different dimensions!");
            Matrix temp = new Matrix(m1.Rows, m1.Columns);
            for (int i = 0; i < m1.Rows; i++)
            {
                for (int j = 0; j < m1.Columns; j++)
                {
                    temp[i, j] = (m1[i, j] < m2[i, j]) ? 1 : 0;
                }
            }
            return temp;
        }

        public static Matrix operator >=(Matrix m1, double value)
        {
            // Returns an array evaluated element by element against m1, and m2
            Matrix temp = new Matrix(m1.Rows, m1.Columns);
            for (int i = 0; i < m1.Rows; i++)
            {
                for (int j = 0; j < m1.Columns; j++)
                {
                    temp[i, j] = (m1[i, j] >= value) ? 1 : 0;
                }
            }
            return temp;
        }

        public static Matrix operator >=(Matrix m1, Matrix m2)
        {
            // Returns an array evaluated element by element against m1, and m2
            if ((m1.Rows != m2.Rows) || (m1.Columns != m2.Columns))
                throw new InvalidMatrixOperationException("Cannot Compare matrices of different dimensions!");
            Matrix temp = new Matrix(m1.Rows, m1.Columns);
            for (int i = 0; i < m1.Rows; i++)
            {
                for (int j = 0; j < m1.Columns; j++)
                {
                    temp[i, j] = (m1[i, j] >= m2[i, j]) ? 1 : 0;
                }
            }
            return temp;
        }

        public static Matrix operator <=(Matrix m1, double value)
        {
            // Returns an array evaluated element by element against m1, and m2
            Matrix temp = new Matrix(m1.Rows, m1.Columns);
            for (int i = 0; i < m1.Rows; i++)
            {
                for (int j = 0; j < m1.Columns; j++)
                {
                    temp[i, j] = (m1[i, j] <= value) ? 1 : 0;
                }
            }
            return temp;
        }

        public static Matrix operator <=(Matrix m1, Matrix m2)
        {
            // Returns an array evaluated element by element against m1, and m2
            if ((m1.Rows != m2.Rows) || (m1.Columns != m2.Columns))
                throw new InvalidMatrixOperationException("Cannot Compare matrices of different dimensions!");
            Matrix temp = new Matrix(m1.Rows, m1.Columns);
            for (int i = 0; i < m1.Rows; i++)
            {
                for (int j = 0; j < m1.Columns; j++)
                {
                    temp[i, j] = (m1[i, j] <= m2[i, j]) ? 1 : 0;
                }
            }
            return temp;
        }

        public static Matrix operator ==(Matrix m1, Matrix m2)
        {
            // Returns an array evaluated element by element against m1, and m2
            if ((m1.Rows != m2.Rows) || (m1.Columns != m2.Columns))
                throw new InvalidMatrixOperationException("Cannot Compare matrices of different dimensions!");
            Matrix temp = new Matrix(m1.Rows, m1.Columns);
            for (int i = 0; i < m1.Rows; i++)
            {
                for (int j = 0; j < m1.Columns; j++)
                {
                    temp[i, j] = (m1[i, j] == m2[i, j]) ? 1 : 0;
                }
            }
            return temp;
        }

        // override object.Equals
        public override bool Equals(object obj)
        {
            //       
            // See the full list of guidelines at
            //   http://go.microsoft.com/fwlink/?LinkID=85237  
            // and also the guidance for operator== at
            //   http://go.microsoft.com/fwlink/?LinkId=85238
            //

            if (obj == null || GetType() != obj.GetType())
            {
                return false;
            }

            Matrix m2 = (Matrix)obj;

            if ((Rows != m2.Rows) || (Columns != m2.Columns))
                throw new InvalidMatrixOperationException("Cannot Compare matrices of different dimensions!");

            // Performs the mathematical operation of subtracting a constant from a matrix
            Matrix temp = new Matrix();
            for (int i = 0; i < Rows; i++)
            {
                for (int j = 0; j < Columns; j++)
                {
                    temp[i, j] = (this[i, j] == m2[i, j]) ? 1 : 0;
                }
            }
            return base.Equals(temp);
        }

        // override object.GetHashCode
        public override int GetHashCode()
        {
            // TODO: write your implementation of GetHashCode() here
            //throw new NotImplementedException();
            return base.GetHashCode();
        }

        public static Matrix operator !=(Matrix m1, Matrix m2)
        {
            // Returns an array evaluated element by element against m1, and m2
            if ((m1.Rows != m2.Rows) || (m1.Columns != m2.Columns))
                throw new InvalidMatrixOperationException("Cannot Compare matrices of different dimensions!");
            Matrix temp = new Matrix(m1.Rows, m1.Columns);
            for (int i = 0; i < m1.Rows; i++)
            {
                for (int j = 0; j < m1.Columns; j++)
                {
                    temp[i, j] = (m1[i, j] != m2[i, j]) ? 1 : 0;
                }
            }
            return temp;
        }

        public static Matrix operator ==(Matrix m1, double value)
        {
            // Returns an array evaluated element by element against m1, and m2
            Matrix temp = new Matrix(m1.Rows, m1.Columns);
            for (int i = 0; i < m1.Rows; i++)
            {
                for (int j = 0; j < m1.Columns; j++)
                {
                    temp[i, j] = (m1[i, j] == value) ? 1 : 0;
                }
            }
            return temp;
        }

        public static Matrix operator !=(Matrix m1, double value)
        {
            // Returns an array evaluated element by element against m1, and m2
            Matrix temp = new Matrix(m1.Rows, m1.Columns);
            for (int i = 0; i < m1.Rows; i++)
            {
                for (int j = 0; j < m1.Columns; j++)
                {
                    temp[i, j] = (m1[i, j] != value) ? 1 : 0;
                }
            }
            return temp;
        }

        // Methods

        public static Matrix Mult(Matrix m1, Matrix m2)
        {
            // Performs the mathematical operation of multiplying two matrices via their elements
            if ((m1.Rows != m2.Rows) || (m1.Columns != m2.Columns))
                throw new InvalidMatrixOperationException("Cannot Multiply matrices of different dimensions!");
            Matrix temp = new Matrix(m1.Rows, m1.Columns);
            for (int i = 0; i < m1.Rows; i++)
            {
                for (int j = 0; j < m1.Columns; j++)
                {
                    temp[i, j] = m1[i, j] * m2[i, j];
                }
            }
            return temp;
        }

        public static Matrix Div(Matrix m1, Matrix m2)
        {
            // Performs the mathematical operation of dividing two matrices via their elements
            if ((m1.Rows != m2.Rows) || (m1.Columns != m2.Columns))
                throw new InvalidMatrixOperationException("Cannot Divide matrices of different dimensions!");
            Matrix temp = new Matrix(m1.Rows, m1.Columns);
            for (int i = 0; i < m1.Rows; i++)
            {
                for (int j = 0; j < m1.Columns; j++)
                {
                    temp[i, j] = m1[i, j] / m2[i, j];
                }
            }
            return temp;
        }

        public void PrintContents()
        {
            // Prints the contents of the matrix to the screen...
            Console.WriteLine("This matrix's contents are:");
            string temp = "[";
            for (int i = 0; i < Rows; i++)
            {
                temp += "[";
                for (int j = 0; j < Columns; j++)
                {
                    temp += this[i, j];
                    if (j != (Columns - 1))
                        temp += ",";
                }
                temp += "]";
                if (i != (Rows - 1))
                    temp += ",\n";
            }
            temp += "]";
            Console.WriteLine(temp);
        }

        public static Matrix CreateColumnMatrix(int length, double DefaultData = 0)
        {
            // Creates a new column matrix
            //Console.WriteLine("Created column vector with length {0}", length);
            return new Matrix(length, 1, DefaultData);
        }

        public static Matrix CreateColumnMatrix(double[] data)
        {
            // Creates a new column matrix
            Matrix temp = new Matrix(data.Length, 1, 0);
            string tempDisp = "[";
            for (int i = 0; i < data.Length; i++)
            {
                temp[i, 0] = data[i];
                tempDisp += data[i];
                if (i != (data.Length - 1))
                    tempDisp += ",";
            }
            tempDisp += "]";
            //Console.WriteLine("Created column vector with length {0}", data.Length);
            //Console.WriteLine("Its contents are: " + tempDisp);
            return temp;
        }

        public static Matrix CreateRowMatrix(int length, double DefaultData = 0)
        {
            // Creates a new row matrix
            //Console.WriteLine("Created row vector with length {0}", length);
            return new Matrix(1, length, DefaultData);
        }

        public static Matrix CreateRowMatrix(double[] data)
        {
            // Creates a new row matrix
            Matrix temp = new Matrix(1, data.Length, 0);
            string tempDisp = "[";
            for (int i = 0; i < data.Length; i++)
            {
                temp[0, i] = data[i];
                tempDisp += data[i];
                if (i != (data.Length - 1))
                    tempDisp += ",";
            }
            tempDisp += "]";
            //Console.WriteLine("Created row vector with length {0}", data.Length);
            //Console.WriteLine("Its contents are: " + tempDisp);
            return temp;
        }

        public static double Dot(double[] m1, double[] m2)
        {
            // Determines the Dot product of two matrices
            double temp = 0;
            if (!(m1.Length == m2.Length))
                throw new InvalidMatrixOperationException("Cannot compute the dot product of matrices of different sizes!");

            for (int i = 0; i < m1.Length; i++)
            {
                temp += m1[i] * m2[i];
            }
            return temp;
        }

        public static Matrix Outer(double[] m1, double[] m2)
        {
            // Calculates the outer product (tensor multiplication) of two vectors
            Matrix temp = new Matrix(m1.Length, m2.Length);
            for(int i = 0; i < m1.Length; i++)
            {
                for(int j = 0; j < m2.Length; j++)
                {
                    temp[i, j] = m1[i] * m2[j];
                }
            }
            return temp;
        }

        public void Clear()
        {
            // Resets all of the data to 0
            for (int i = 0; i < Rows; i++)
            {
                for (int j = 0; j < Columns; j++)
                {
                    this[i, j] = 0;
                }
            }
        }

        public Matrix Clone()
        {
            // Returns a copy of this matrix instance's data
            Matrix temp = new Matrix(data.Rows, data.Columns, defaultData);
            for(int i = 0; i < Rows; i++)
            {
                for(int j = 0; j < Columns; j++)
                {
                    double tempVal = this[i, j];
                    temp[i, j] = tempVal;
                }
            }
            //temp.PrintContents();
            return temp;
        }

        public double[] GetRow(int index)
        {
            return data.GetRow(index);
        }

        public double[] GetCol(int index)
        {
            return data.GetColumn(index);
        }

        public bool IsVector()
        {
            // Returns true if the matrix contains either only 1 row, or only 1 column
            if ((Rows == 1) || (Columns == 1))
                return false;
            return false;
        }

        public bool IsZero()
        {
            // Returns true if all elements in the matrix are zero
            foreach (double item in data)
                if (item != 0)
                    return false;
            return true;
        }

        public double Sum()
        {
            // returns the cumulative sum of all elements in the matrix
            double total = 0;
            foreach(double item in data)
            {
                total += item;
            }
            return total;
        }

        public double[] ToPackedArray()
        {
            // Returns the matrix as a single dimensional array
            double[] temp = new double[Size];
            int i = 0;
            foreach(double item in data)
            {
                temp[i++] = item;
            }
            return temp;
        }

        public List<List<double>> ColData()
        {
            return data.ColumnData;
        }

        public List<List<double>> RowData()
        {
            return data.RowData;
        }

        public Matrix GetSubMatrix(int[] rowExc, int[] colExc, int rowOffset = 0, int colOffset = 0)
        {
            // Returns a submatrix of the original excluding rowExc, and colExc and starting at row and column offsets
            if (((rowOffset + rowExc.Length) > Rows) || ((colOffset + colExc.Length) > Columns))
                throw new InvalidMatrixOperationException("Offsets cannot be greater than dimensions of matrix");

            Matrix temp = new Matrix(Rows - rowExc.Length, Columns - colExc.Length);
            for(int i = 0; i < Rows; i++)
            {
                if(rowExc.Contains(i))
                {
                    rowOffset++;
                    continue;
                }
                colOffset = 0;
                for(int j = 0; j < Columns; j++)
                {
                    
                    if(colExc.Contains(j))
                    {
                        if (colExc.Contains(j))
                            colOffset++;
                        continue;
                    }
                    temp[(i - rowOffset), (j - colOffset)] = data[i, j];
                }
            }
            return temp;
        }

        public double Determinant()
        {
            // Determines the determinant of a matrix
            if (Rows != Columns)
                throw new InvalidMatrixOperationException("Cannot find the determinant of a matrix that isn't square");

            //Console.WriteLine("Began a determinant calculation...");
            double total = 0;
            int iter = 1;
            List<List<double>> columns = ColData();

            if (Size == 1)
                return data[0, 0];
            else if ((Rows == 2) && (Columns == 2))
                return Det2x2();
            else
            {
                // Begins a multi-chain call that reciprocates between SubDeterminant and Determinant, ultimately ending with a call to Det2x2 which completes the chain.
                for(int i = 0; i < data.Columns; i++)
                {
                    total += iter * columns[i][0] * SubDeterminant();
                    iter *= -1;
                }
            }

            return total;
        }

        private double SubDeterminant()
        {
            // Intermediary for Determinant function
            // Extrapolates the submatrix from the current one and continues the determinant calculation
            Matrix temp = GetSubMatrix(new int[] { 0 }, new int[] { 0 });
            return temp.Determinant();
        }

        private double Det2x2()
        {
            // Helper for the Determinant function
            // Finds the determinant of a 2x2 matrix
            return (this[0, 0] * this[1, 1]) - (this[0, 1] * this[1, 0]);
        }

        public Matrix Identity(IdentityMult position)
        {
            // Returns the identity matrix of the current matrix
            Matrix temp;
            int Length = 0;

            // Determines the dimensions of the matrix
            switch(position)
            {
                case (IdentityMult.BEFORE):
                    Length = Rows;
                    break;
                case (IdentityMult.AFTER):
                    Length = Columns;
                    break;
                default:
                    throw new IndexOutOfRangeException();
            }

            // Generates the matrix
            temp = new Matrix(Length, Length);
            for (int i = 0; i < Length; i++)
            {
                temp[i, i] = 1;
            }

            return temp;
        }

        public Matrix Transpose()
        {
            // Transposes a 2 dimensional array
            Matrix temp = new Matrix(ColData().Count, RowData().Count);
            for(int i = 0; i < ColData().Count; i++)
            {
                for(int j = 0; j < RowData().Count; j++)
                {
                    temp[i, j] = this[j, i];
                }
            }
            return temp;
        }

        public Matrix Invert()
        {
            // Inverts this matrix
            Matrix Minors = new Matrix(Rows, Columns);

            // Calculates the matrix of minors and applies the matrix of cofactors to it simeaultaneously
            double iter = 1;
            for(int i = 0; i < Rows; i++)
            {
                for(int j = 0; j < Columns; j++)
                {
                    Matrix temp = GetSubMatrix(new int[] { i }, new int[] { j });
                    Minors[i, j] = iter * temp.Determinant();
                    iter *= -1;
                }
            }

            // Adjugates the matrix of minors and divides it by the determinant of the original
            return Minors.Transpose() / Determinant();
        }

        public double EucNorm()
        {
            // Calculates the Euclidean Norm of a matrix (Magnitude)
            double temp = 0;
            foreach(double item in ToPackedArray())
            {
                temp += item * item;
            }
            return Math.Sqrt(temp);
        }

        // Exceptions
        public class InvalidMatrixOperationException : Exception
        {
            public InvalidMatrixOperationException()
            {
                
            }

            public InvalidMatrixOperationException(string message) : base(message)
            {
                Console.WriteLine(message);
            }

            public InvalidMatrixOperationException(string message, Exception inner) : base(message, inner)
            {
                Console.WriteLine(message);
                throw inner;
            }
        }
    }
}
