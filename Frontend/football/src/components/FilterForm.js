import React, { useState } from 'react';

const FilterForm = ({ onClose, onFilter, filterOptions }) => {
  const [name, setName] = useState('');
  const [position, setPosition] = useState('');
  const [nationality, setNationality] = useState('');
  const [sortBy, setSortBy] = useState('Name');
  const [sortOrder, setSortOrder] = useState('ASC');
  const [pageSize, setPageSize] = useState(10);
  const [pageNumber, setPageNumber] = useState(1);

  const handleSubmit = (e) => {
    e.preventDefault();
    const filters = {
      Name: name,
      Position: position,
      Nationality: nationality,
      OrderBy: sortBy,
      OrderDirection: sortOrder,
      PageSize: pageSize,
      PageNumber: pageNumber,
    };
    onFilter(filters);
    onClose();
  };

  return (
    <div className="filter-form">
      <h3>Filter Players</h3>
      <form onSubmit={handleSubmit}>
        <label>
          Name:
          <input 
            type="text" 
            value={name} 
            onChange={(e) => setName(e.target.value)} 
            className="filter-input" 
          />
        </label>
        <br />
        <label>
          Position:
          <select 
            value={position} 
            onChange={(e) => setPosition(e.target.value)} 
            className="filter-input"
          >
            <option value="">Select Position</option>
            {filterOptions.positions.map(option => (
              <option key={option} value={option}>{option}</option>
            ))}
          </select>
        </label>
        <br />
        <label>
          Nationality:
          <select 
            value={nationality} 
            onChange={(e) => setNationality(e.target.value)} 
            className="filter-input"
          >
            <option value="">Select Nationality</option>
            {filterOptions.nationalities.map(option => (
              <option key={option} value={option}>{option}</option>
            ))}
          </select>
        </label>
        <br />
        <label>
          Sort By:
          <select 
            value={sortBy} 
            onChange={(e) => setSortBy(e.target.value)} 
            className="filter-input"
          >
            <option value="Name">Name</option>
            <option value="Position">Position</option>
            <option value="Number">Number</option>
            <option value="Age">Age</option>
            <option value="Nationality">Nationality</option>
          </select>
        </label>
        <br />
        <label>
          Sort Order:
          <select 
            value={sortOrder} 
            onChange={(e) => setSortOrder(e.target.value)} 
            className="filter-input"
          >
            <option value="ASC">Ascending</option>
            <option value="DESC">Descending</option>
          </select>
        </label>
        <br />
        <label>
          Page Size:
          <input 
            type="number" 
            value={pageSize} 
            onChange={(e) => setPageSize(e.target.value)} 
            className="filter-input" 
          />
        </label>
        <br />
        <label>
          Page Number:
          <input 
            type="number" 
            value={pageNumber} 
            onChange={(e) => setPageNumber(e.target.value)} 
            className="filter-input" 
          />
        </label>
        <br />
        <button type="submit" className="filter-button">Apply Filter</button>
        <button type="button" onClick={onClose} className="close-button">Close</button>
      </form>
    </div>
  );
};

export default FilterForm;
