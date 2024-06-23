const requestHandler = async (url, options) => {
    try {
      const response = await fetch(url, options);
      if (!response.ok) throw new Error('An error occurred');
      return null;
    } catch (err) {
      return err.message;
    }
  };
  
  export default requestHandler;
  