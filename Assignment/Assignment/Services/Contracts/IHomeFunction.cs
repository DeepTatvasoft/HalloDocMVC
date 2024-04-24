using Services.ViewModels;

namespace Services.Contracts
{
    public interface IHomeFunction
    {
        void DeleteData(int id);
        void EditData(StudentFormModal modal, string gender);
        StudentFormModal EditStudent(int id);
        StudentFormModal Index();
        void SubmitData(StudentFormModal modal, string gender);
    }
}