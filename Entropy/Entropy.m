one = linspace(0, 1, 100);
p1 = zeros(100, 1);
p2 = zeros(100, 1);
p3_m = zeros(100, 100);
H = zeros(100, 100);

for i = 1:100
    for j = 1:(100-i)
        p1(i) = one(i);
        p2(j) = one(j);
        p3 = 1 - p1(i) - p2(j);
        p3_m(i, j) = p3;
        H(i, j) = -(p1(i) * log2(p1(i))) - (p2(j) * log2(p2(j))) - (p3 * log2(p3));
    end
end

figure(1);
[p1, p2] = meshgrid(p1, p2);
mesh (p1, p2, H);
xlabel('p1');
ylabel('p2');
zlabel('H');

figure(2);
mesh (p1, p2, p3_m);
xlabel('p1');
ylabel('p2');
zlabel('p3');
 
maximum = max(H(:))
