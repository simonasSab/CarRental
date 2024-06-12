﻿namespace Uzduotis01
{
    public interface IDatabaseRepository
    {
        bool GetAllVehicles(out IEnumerable<FossilFuelVehicle> fossilFuelVehicles, out IEnumerable<ElectricVehicle> electricVehicles);
        bool GetAllVehicles(string phrase, out IEnumerable<FossilFuelVehicle> fossilFuelVehicles, out IEnumerable<ElectricVehicle> electricVehicles); // For search
        IEnumerable<Vehicle> GetAllVehicles(bool isElectric);
        IEnumerable<Client> GetAllClients();
        IEnumerable<Client> GetAllClients(string phrase); // For search
        IEnumerable<Rent> GetAllRents();
        IEnumerable<Rent>? GetAllRents(int vehicleID);

        Vehicle GetVehicle(int ID);
        Client GetClient(int ID);
        Rent GetRent(int ID);

        bool InsertVehicle(ElectricVehicle vehicle, out ElectricVehicle newVehicle);
        bool InsertVehicle(FossilFuelVehicle vehicle, out FossilFuelVehicle newVehicle);
        bool InsertClient(Client client, out Client newClient);
        bool InsertRent(Rent rent, out Rent newRent);

        bool DeleteVehicle(int ID);
        bool DeleteClient(int ID);
        bool DeleteRent(int ID);

        bool UpdateVehicle(object? vehicle, out object updatedVehicle);
        bool UpdateClient(Client? client, out Client updatedClient);
        bool UpdateRent(Rent? rent, out Rent updatedRent);

        // Entity Framework
        bool InsertClientEF(Client client, out Client newClient);
        Client? GetClientEF(int ID);
        IEnumerable<Client> GetAllClientsEF();
        IEnumerable<Client> GetAllClientsEF(string phrase); // For search
    }
}
