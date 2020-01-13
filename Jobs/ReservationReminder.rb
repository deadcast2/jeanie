require 'json'
require 'net/http'
require 'uri'

def lambda_handler(event:, context:)
    Net::HTTP.post(
        URI('https://jeanie-reservation-system.com/Jobs/SendReminders'),
        {}.to_json
    )
end